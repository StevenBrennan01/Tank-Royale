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

    public float currentHealth {  get; set; }
    public float maxHealth { get; set; }

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
        maxHealth = tankData.tankMaxHealth;
        currentHealth = tankData.tankCurrentHealth;

        currentHealth = maxHealth;

        if (GetComponent<PlayerController>() != null) // ALWAYS TRUE FOR THE PLAYER
        {
            agentCanRespawn = true;
        }
        else agentCanRespawn = false; // THIS IS ALWAYS AN ENEMY

        isActive = true;

        uiManager_SCR.UpdateHealthUI(this, healthBarImage);

        if (respawnPosition is null)
        {
            Debug.LogError("Player has no respawn position, please set one in the inspector");
        }
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
        if (currentHealth < maxHealth)
        {
            currentHealth += healthIncreased;

            //Stops player from overhealing
            currentHealth = Mathf.Min(currentHealth, maxHealth);

            if (isActive)
            {
                uiManager_SCR.UpdateHealthUI(this, healthBarImage);
            }
        }
    }
}