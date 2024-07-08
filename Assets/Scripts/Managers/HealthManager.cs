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

    [HideInInspector] public bool agentCanRespawn;
    public float respawnDelay;
    public Transform respawnPosition;

    [SerializeField] private float maxHealth;
    public float currentHealth;
    private float newestHealthAmount;
    private float UIDelay;

    private void OnEnable()
    {
        gameManager_SCR = FindObjectOfType<GameManager>();

        currentHealth = maxHealth;
        newestHealthAmount = maxHealth;

        if (GetComponent<PlayerController>() != null)
        {
            agentCanRespawn = true; //THIS IS ALWYAYS THE PLAYER
        }
        else agentCanRespawn = false; //THIS IS ALWAYS AN ENEMY
    }

    private void UpdateHealthUI()
    {
        //healthBarImage.fillAmount = Mathf.Lerp(newestHealthAmount, currentHealth / maxHealth, UIDelay);
        healthBarImage.fillAmount = Mathf.Lerp(newestHealthAmount, currentHealth / maxHealth, UIDelay);
    }

    public void IncreaseHealth()
    {
        //healing function
    }

    public void DealDamage(float damageDealt)
    {
        currentHealth -= damageDealt;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Mathf.Clamp01(currentHealth);
            UpdateHealthUI();

            if (respawnPosition != null && agentCanRespawn)
            {
                gameManager_SCR.AgentDeath(this.gameObject, this.respawnPosition, this.respawnDelay);
            }
            else
            {
                //currentEnemyCount --; HERE WE CAN DO WAVES OF ENEMIES OR SOMETHING
                Destroy(this.gameObject);
            }
        }
    }
}