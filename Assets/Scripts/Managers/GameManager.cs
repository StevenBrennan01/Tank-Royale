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

    [SerializeField] private GameObject[] tankEnemies;
    [SerializeField] private Transform[] enemySpawnPositions;

    private int enemySpawnAmount;
    [SerializeField] private int minEnemiesToSpawn;
    [SerializeField] private int maxEnemiesToSpawn;

    private void Awake()
    {

        if (tankEnemies.Length <= 0) Debug.Log("No enemies assigned, please assign some enemies to the level");
        if (enemySpawnPositions.Length <= 0) Debug.Log("Please assign some locations for enemies to spawn");
    }

    void Start()
    {
        //spawn enemies, play music, etc.
    }
}
