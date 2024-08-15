using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
    #endregion

    #region Game UI
    [SerializeField] private TMP_Text enemiesRemainingText;
    #endregion

    private Coroutine entityDeath_CR;

    #region Inspector Header and Spacing
    [Space(15)]
    [Header("                                                     -= Respawn Manager =-")]
    [Space(15)]
    #endregion

    [SerializeField] private int maxLives = 3;
    [SerializeField] private int currentLife;

    private void Awake()
    {
        if (tankEnemies.Length <= 0) Debug.LogError("No enemies assigned, please assign some enemies to the level");
        if (enemySpawnPositions.Length <= 0) Debug.LogError("Please assign some locations for enemies to spawn");

        healthManager_SCR = FindObjectOfType<HealthManager>();
        uiManager_SCR = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        SpawnEnemies();

        CurrentEnemyCountUI();

        currentLife = maxLives;
        //play music, etc.
    }

    private void SpawnEnemies()
    {
        enemyCount = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn);

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

    public void EnemyDeath()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            CurrentEnemyCountUI();
            // do something, display ui ...

            Debug.Log("All enemies are dead");
        }
        else
        {
            CurrentEnemyCountUI();
        }
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}