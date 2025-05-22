using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private HealthManager healthManager_SCR;
    private UIManager uiManager_SCR;

    #region Inspector Header and Spacing
    [Header("                                                     -= Enemy Manager =-")]
    [Space(15)]
    #endregion

    #region Enemy Manager
    [SerializeField] private GameObject[] tankEnemies;
    [SerializeField] private Transform[] enemySpawnPositions;

    public int enemyCount;
    [SerializeField] private int minEnemiesToSpawn;
    [SerializeField] private int maxEnemiesToSpawn;
    [SerializeField] private int playerScore = 0;
    private int playerScorePerKill;

    public int waveCount;
    private int maxWave = 4; //then final boss wave or something / gradually increase enemies per wave

    #endregion

    #region Game UI
    [SerializeField] private TMP_Text enemiesRemainingText;
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private TMP_Text playerScoreText;
    #endregion

    private Coroutine entityDeath_CR;
    private Coroutine reinstateHealthPickup; // not implemented yet 

    #region Inspector Header and Spacing
    [Space(15)]
    [Header("                                                     -= Respawn Manager =-")]
    [Space(15)]
    #endregion

    [SerializeField] private int maxLives = 3;
    [SerializeField] private int currentLife;

    private void Awake()
    {
        //// Singleton pattern
        //if(instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        //else { Destroy(this.gameObject); }

        if (tankEnemies.Length <= 0) Debug.LogError("No enemies assigned, please assign some enemies to the level");
        if (enemySpawnPositions.Length <= 0) Debug.LogError("Please assign some locations for enemies to spawn");

        healthManager_SCR = FindObjectOfType<HealthManager>();
        uiManager_SCR = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        if (instance != null) { Destroy(this.gameObject); }
        else { instance = this; }

        waveCount = 1;

        // Resetting the game

        currentLife = maxLives;
        playerScore = 0;

        SpawnEnemies();

        CurrentWaveNumber();
        CurrentEnemyCountUI();
        CurrentPlayerScore(0);

        //play music, etc.
    }

    private void SpawnEnemies()
    {
        switch(waveCount)
        {
            case 1:
                enemyCount = Random.Range(1, 3);
                break;
            case 2:
                enemyCount = Random.Range(3, 5);
                break;
            case 3:
                enemyCount = Random.Range(5, 7);
                break;
            case 4:
                enemyCount = Random.Range(7, 10);
                break;
            default:
                enemyCount = Random.Range(0,0);
                break;
        }
        CurrentEnemyCountUI();

        // Randomly shuffles through the spawnPositions Array
        List<Transform> shufflePositions = enemySpawnPositions.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < enemyCount; i++)
        {
            if (i >= shufflePositions.Count) break; // Allows only 1 to be spawned per Position

            Transform spawnPosition = shufflePositions[i];
            GameObject tankEnemy = tankEnemies[Random.Range(0, tankEnemies.Length)];
            Instantiate(tankEnemy, spawnPosition.position, Quaternion.identity);
        }
    }

    private void CurrentEnemyCountUI()
    {
        enemiesRemainingText.text = enemyCount.ToString();
    }

    private void CurrentWaveNumber()
    {
        waveNumberText.text = waveCount.ToString();
    }

    public void CurrentPlayerScore(int killScore)
    {
        playerScore += killScore;
        playerScoreText.text = playerScore.ToString();
    }

    public void EnemyDeath()
    {
        enemyCount--;

        CurrentPlayerScore(Random.Range(2,4));

        if (enemyCount <= 0)
        {
            CurrentEnemyCountUI();
            // do something, display ui ...

            Debug.Log("All enemies are dead");

            // Freeze the player for 3 seconds

            StartCoroutine(EndOfRound(healthManager_SCR.gameObject, 3f));

            //////// START NEXT WAVE OF ENEMY TANKS
            //if (waveCount < maxWave)
            //{
            //    Debug.Log("Next Wave Starting");
            //    waveCount++;

            //    // Updating the UI
            //    CurrentWaveNumber();
            //    // Generating new Enemies
            //    SpawnEnemies();
            //}
            //else
            //{
            //    Debug.Log("Final Boss Wave");
            //    // Spawn final boss or something
            //}
        }
        else
        {
            CurrentEnemyCountUI();
        }
    }

    private void WaveChecker()
    {
        if (waveCount < maxWave)
        {
            Debug.Log("Next Wave Starting");
            waveCount++;

            // Updating the UI
            CurrentWaveNumber();
            // Generating new Enemies
            SpawnEnemies();
        }
        else
        {
            Debug.Log("Final Boss Wave");
            // Spawn final boss or something
        }
    }

    private IEnumerator EndOfRound(GameObject Agent, float freezeTime)
    {
        Agent.GetComponent<PlayerController>().rb.velocity = Vector2.zero;
        Agent.GetComponent<PlayerController>().rb.isKinematic = true;

        //Show the end of round UI here, countdown etc.

        yield return new WaitForSeconds(freezeTime);
        AgentReset(healthManager_SCR.gameObject, healthManager_SCR.respawnPosition);
    }

    public void AgentReset(GameObject Agent, Transform respawnPosition)
    {
        Agent.transform.position = respawnPosition.position;

        uiManager_SCR.ReloadAmmoUI();

        // Wave starting ui here

        //Coroutine to freeze player for 3 seconds
        StartCoroutine(StartOfRound(Agent, 3f));
    }

    private IEnumerator StartOfRound(GameObject Agent, float freezeTime)
    {
        WaveChecker();
        Debug.Log("Player is frozen for " + freezeTime + " seconds");

        yield return new WaitForSeconds(freezeTime);

        Agent.GetComponent<PlayerController>().rb.isKinematic = false;
        Debug.Log("Player is unfrozen");
    }

    public void AgentDeath(GameObject Agent, Transform respawnPosition, float respawnDelay, HealthManager target, Image healthBarImage)
    {
        currentLife--;

        uiManager_SCR.DepleteLives();

        if (currentLife <= 0)
        {
            Debug.Log("Player has no lives left");

            Agent.SetActive(false);

            //Show the death screen ui here, replay level etc.
        }
        else
        {
            entityDeath_CR = StartCoroutine(AgentDeath_CR(Agent, respawnPosition, respawnDelay, target, healthBarImage));
        }
    }

    // ==== COROUTINES ====

    private IEnumerator AgentDeath_CR(GameObject Agent, Transform respawnPosition, float respawnDelay, HealthManager healthScript, Image healthBarImage)
    {
        uiManager_SCR.UpdateHealthUI(healthScript, healthBarImage);

        Agent.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ACTIVATE SOME RESPAWN UI

        yield return new WaitForSeconds(respawnDelay);
        Agent.transform.position = respawnPosition.position;

        uiManager_SCR.ReloadAmmoUI();
        Agent.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}