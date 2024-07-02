using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    #region Inspector Comments and Spacing
    [Header("-= Health Manager =-")]
    [Space(10)]
    #endregion

    private GameManager gameManager_SCR;

    [SerializeField] private Image healthBar;

    [SerializeField] bool canRespawn;

    private float maxHealth;
    private float currentHealth;


    private void OnEnable()
    {
        gameManager_SCR = FindObjectOfType<GameManager>();

        currentHealth = maxHealth;
        
    }
}
