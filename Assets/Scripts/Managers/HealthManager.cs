using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private GameManager gameManager_SCR;
    private UIManager uiManager_SCR;

    #region Inspector Header and Spacing
    [Header("                                                          -= Health Manager =-")]
    [Space(15)]
    #endregion

    public Image healthBarImage;

    private bool agentCanRespawn;
    private float respawnDelay = 3f;
    public Transform respawnPosition;

    public float maxHealth;
    public float currentHealth;

    public float uiDelay = .1f;

    private bool isActive;

    private void Awake()
    {
        gameManager_SCR = FindObjectOfType<GameManager>();
        uiManager_SCR = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;

        if (GetComponent<PlayerController>() != null) // ALWYAYS TRUE FOR THE PLAYER
        {
            agentCanRespawn = true;
        }
        else agentCanRespawn = false; // THIS IS ALWAYS AN ENEMY

        isActive = true;

        uiManager_SCR.UpdateHealthUI(this, healthBarImage);
    }

    public void DealDamage(float damageDealt)
    {
        currentHealth -= damageDealt;

        if (currentHealth <= 0) // If the player or enemy is dead
        {
            Mathf.Clamp01(currentHealth);
            isActive = false;

            if (respawnPosition != null && agentCanRespawn) // aka is the player
            {
                gameManager_SCR.AgentDeath(this.gameObject, this.respawnPosition, this.respawnDelay, this, healthBarImage);
            }
            else // aka is not the player
            {
                //currentEnemyCount --;

                Destroy(this.gameObject); 
            }
        }

        if (isActive)
        {
            uiManager_SCR.UpdateHealthUI(this, healthBarImage);
        }
    }

    public void IncreaseHealth(float healthIncreased)
    {
        currentHealth += healthIncreased;

        if (isActive)
        {
            uiManager_SCR.UpdateHealthUI(this, healthBarImage);
        }
    }
}