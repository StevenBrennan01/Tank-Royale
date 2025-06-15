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
    private PlayerController playerController_SCR;
    private InputManager inputManager_SCR;
    private ProjectileHandler projectileHandler_SCR;

    #region Inspector Header and Spacing
    [Header("                                                     -= Enemy Manager =-")]
    [Space(15)]
    #endregion

    #region Enemy Manager
    [SerializeField] public GameObject[] tankEnemies;
    [SerializeField] private Transform[] enemySpawnPositions;
    [SerializeField] private GameObject tankBoss;

    //Populate this list with the enemies as soon as they spawn in
    public List<GameObject> activeEnemies = new List<GameObject> ();

    public int enemyCount;
    //[SerializeField] private int minEnemiesToSpawn;
    //[SerializeField] private int maxEnemiesToSpawn;
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

    [SerializeField] private Transform resetPos;

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
        playerController_SCR = FindObjectOfType<PlayerController>();
        inputManager_SCR = FindObjectOfType<InputManager>();
        projectileHandler_SCR = FindObjectOfType<ProjectileHandler>();
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

        StartCoroutine(StartOfGame(playerController_SCR.gameObject, 6f));
    }

    private void WaveChecker()
    {
        if (waveCount < maxWave)
        {
            waveCount++;

            // Updating the UI
            CurrentWaveNumber();

            // Generating new Enemies
            SpawnEnemies();
        }
        else
        {
            SpawnBoss();

            uiManager_SCR.StartBossCountdown();
        }
    }

    private void SpawnBoss()
    {
        if (tankBoss != null)
        {
            Instantiate(tankBoss, enemySpawnPositions[5].position, Quaternion.identity);
            Debug.Log("Final Boss Spawned");
        }
        else
        {
            Debug.LogError("No tank boss assigned, please assign a tank boss to the level");
        }
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
            GameObject newlySpawnedEnemy = Instantiate(tankEnemy, spawnPosition.position, Quaternion.identity);

            // Add the spawned enemy to the active enemies list
            activeEnemies.Add(newlySpawnedEnemy);
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
        //activeEnemies.Remove(activeEnemies.LastOrDefault());

        CurrentPlayerScore(Random.Range(2,4));

        if (enemyCount <= 0)
        {
            CurrentEnemyCountUI();
            // do something, display ui ...

            Debug.Log("All enemies are dead");

            // Freeze the player for 3 seconds

            StartCoroutine(EndOfRound(playerController_SCR.gameObject, 3f));
        }
        else
        {
            CurrentEnemyCountUI();
        }
    }

    public void AgentReset(GameObject Agent)
    {
        Agent.SetActive(false);
        Agent.transform.position = resetPos.position;
        Agent.SetActive(true);

        inputManager_SCR.tankReloadingCR();

        //Next Wave UI
        uiManager_SCR.StartCountdownTimer();

        //Coroutine to freeze player for 3 seconds
        StartCoroutine(StartOfRound(Agent, 3f));
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

    private IEnumerator StartOfGame(GameObject Agent, float freezeTime)
    {
        //Display Start of Game UI
        Agent.GetComponent<PlayerController>().rb.velocity = Vector2.zero;
        Agent.GetComponent<PlayerController>().rb.isKinematic = true;

        projectileHandler_SCR.canFire = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        uiManager_SCR.StartLevelUI();

        yield return new WaitForSeconds(freezeTime);
        Agent.GetComponent<PlayerController>().rb.isKinematic = false;

        projectileHandler_SCR.canFire = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

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

    private IEnumerator EndOfRound(GameObject Agent, float freezeTime)
    {
        projectileHandler_SCR.canReload = false;

        Agent.GetComponent<PlayerController>().rb.velocity = Vector2.zero;
        Agent.GetComponent<PlayerController>().rb.isKinematic = true;

        activeEnemies.Clear();

        yield return new WaitForSeconds(freezeTime);

        projectileHandler_SCR.canReload = true;
        AgentReset(playerController_SCR.gameObject);
    }

    private IEnumerator StartOfRound(GameObject Agent, float freezeTime)
    {
        WaveChecker();

        if (uiManager_SCR.reloadUI.activeSelf)
        {
            uiManager_SCR.reloadUI.SetActive(false);
        }

        currentLife = maxLives; 
        uiManager_SCR.IncreaseLives();

        Debug.Log("Player is frozen for " + freezeTime + " seconds");

        yield return new WaitForSeconds(freezeTime);

        Agent.GetComponent<PlayerController>().rb.isKinematic = false;
        Debug.Log("Player is unfrozen");
    }
}