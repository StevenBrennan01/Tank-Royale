using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Inspector Header and Spacing
    [Header("                                                     -= Enemy Manager =-")]
    [Space(15)]
    #endregion

    #region Enemy Manager
    [SerializeField] private GameObject[] tankEnemies;
    [SerializeField] private Transform[] enemySpawnPositions;

    private int enemySpawnAmount;
    [SerializeField] private int minEnemiesToSpawn;
    [SerializeField] private int maxEnemiesToSpawn;
    #endregion

    private Coroutine EntityDeath_CR;

    #region Inspector Header and Spacing
    [Space(15)]
    [Header("                                                     -= Respawn Manager =-")]
    [Space(15)]
    #endregion

    [SerializeField] private GameObject respawnUI;

    private void Awake()
    {
        if (tankEnemies.Length <= 0) Debug.LogError("No enemies assigned, please assign some enemies to the level");
        if (enemySpawnPositions.Length <= 0) Debug.LogError("Please assign some locations for enemies to spawn");
    }

    void Start()
    {
        //spawn enemies at spawn positions
        //play music, etc.
    }

        //METHODS

    public void AgentDeath(GameObject Agent, Transform respawnPosition, float respawnDelay)
    {
        EntityDeath_CR = StartCoroutine(AgentDeath_CR(Agent, respawnPosition, respawnDelay));
    }

        //COROUTINES

    private IEnumerator AgentDeath_CR(GameObject Agent, Transform respawnPosition, float respawnDelay)
    {
        Agent.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ACTIVATE SOME RESPAWN UI

        yield return new WaitForSeconds(respawnDelay);
        Agent.transform.position = respawnPosition.position;

        // GIVE AGENT MAX HEALTH AND AMMO AGAIN

        // DISABLE RESPAWN UI
        Agent.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
