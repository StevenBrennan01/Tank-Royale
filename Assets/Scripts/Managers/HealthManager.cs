using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private GameManager gameManager_SCR;

    #region Inspector Comments and Spacing
    [Header("                                                          -= Health Manager =-")]
    [Space(15)]
    #endregion

    [SerializeField] private Image healthBarImage;

    [SerializeField] private bool agentCanRespawn;
    [SerializeField] private float respawnDelay;

    [SerializeField] private float maxHealth;
    private float currentHealth;

    private void OnEnable()
    {
        gameManager_SCR = FindObjectOfType<GameManager>();

        currentHealth = maxHealth;
    }

    public void IncreaseHealth()
    {
        //healing function
    }

    public void DealDamage(float damageDealt)
    {
        
    }


}