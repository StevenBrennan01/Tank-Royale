using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private GameManager gameManager_SCR;
    private UIManager uiManager_SCR;

    //TankData
    [SerializeField]
    private TankAttributesSO tankData;

    #region Inspector Header and Spacing
    [Header("                                                          -= Health Manager =-")]
    [Space(15)]
    #endregion

    public Image healthBarImage;

    private bool agentCanRespawn;
    private float respawnDelay = 3f;
    public Transform respawnPosition;

    public float maxHealth;
    //public float currentHealth;

    public float uiDelay = .1f;

    private bool isActive;

    private void Awake()
    {
        gameManager_SCR = FindObjectOfType<GameManager>();
        uiManager_SCR = FindObjectOfType<UIManager>();

        if (tankData == null)
        {
            Debug.Log("No tank data has been attached!");
        }
    }

    private void OnEnable()
    {
        tankData.tankHealth = maxHealth;

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
        tankData.tankHealth -= damageDealt;

        if (tankData.tankHealth <= 0) // If the player or enemy is dead
        {
            Mathf.Clamp01(tankData.tankHealth);
            isActive = false;

            if (respawnPosition != null && agentCanRespawn) // aka is the player
            {
                gameManager_SCR.AgentDeath(this.gameObject, this.respawnPosition, this.respawnDelay, this, healthBarImage);
            }
            else // aka is not the player
            {
                gameManager_SCR.EnemyDeath();

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
        if (tankData.tankHealth < maxHealth)
        {
            tankData.tankHealth += healthIncreased;

            //Stops player from overhealing
            tankData.tankHealth = Mathf.Min(tankData.tankHealth, maxHealth);

            if (isActive)
            {
                uiManager_SCR.UpdateHealthUI(this, healthBarImage);
            }
        }
    }
}