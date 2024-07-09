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

    private bool agentCanRespawn;
    private float respawnDelay = 3f;
    public Transform respawnPosition;

    [SerializeField] private float maxHealth;
    private float currentHealth;
    private float UIDelay = .1f;

    private Coroutine smoothHealthBar_CR;

    private void OnEnable()
    {
        gameManager_SCR = FindObjectOfType<GameManager>();

        currentHealth = maxHealth;

        if (GetComponent<PlayerController>() != null) // ALWYAYS TRUE FOR THE PLAYER
        {
            agentCanRespawn = true;
        }
        else agentCanRespawn = false; // THIS IS ALWAYS AN ENEMY

        UpdateHealthUI();
    }

    private void UpdateHealthUI() { smoothHealthBar_CR = StartCoroutine(SmoothHealthBar()); }

        private IEnumerator SmoothHealthBar()
        {
            float currentFillAmount = healthBarImage.fillAmount;
            float targetFillAmount = currentHealth / maxHealth;
            float elapsedTime = 0f;

            while (elapsedTime < UIDelay)
            {
                elapsedTime += Time.deltaTime;

                healthBarImage.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, elapsedTime / UIDelay);
                yield return null;
            }
            healthBarImage.fillAmount = targetFillAmount;
        }

    public void DealDamage(float damageDealt)
    {
        currentHealth -= damageDealt;

        if (currentHealth <= 0)
        {
            Mathf.Clamp01(currentHealth);

            if (respawnPosition != null && agentCanRespawn) // aka is the player
            {
                gameManager_SCR.AgentDeath(this.gameObject, this.respawnPosition, this.respawnDelay);
            }
            else // aka is not the player
            {
                //currentEnemyCount --; HERE WE CAN DO WAVES OF ENEMIES OR SOMETHING
                Destroy(this.gameObject); 
            }
        }

        UpdateHealthUI();

    }

    public void IncreaseHealth()
    {
        //healing function
    }
}