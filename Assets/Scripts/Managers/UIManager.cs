using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private HealthManager healthManager_SCR;
    private ProjectileHandler projectileHandler_SCR;

    #region Inspector Header and Spacing
    [Header("-= UI Manager =-")]
    [Space(15)]
    #endregion

    public GameObject healthUI;
    public GameObject pauseMenuUI;

    [SerializeField] private GameObject[] bulletUI;
    private float reloadRoundUIDelay = .5f;
    // BULLET UI DEPLETION
    // ARRAY OF GAMEOBJECTS FOR THE BULLETS, FOR LOOP CHANGING THE COLOR OF EACH INDEX UNTIL MINIMUM IS REACHED
    // THEN WHEN RELOADING INCREMENT THE OPPOSITE WAY

    private Coroutine smoothHealthBar_CR;

    private void Awake()
    {
        healthManager_SCR = FindObjectOfType<HealthManager>();
        projectileHandler_SCR = FindObjectOfType<ProjectileHandler>();
    }

    //public void AmmoDepleteUI()
    //{
    //    for (int i = 0; i < maxAmmo; i++)
    //    {
           
            
    //        // ADD SFX HERE TO AUDIOLISE RELOADING
    //    }
    //}

    public void UpdateHealthUI(HealthManager target, Image healthBarImage)
    {
        smoothHealthBar_CR = StartCoroutine(SmoothHealthBar(target, healthBarImage));
    }

    //SLOWING DOWN HEALTHBAR UPDATE     //LOOK AT COLOR.LERP FOR HEALTHBAR COLOUR CHANGE?
    private IEnumerator SmoothHealthBar(HealthManager target, Image healthBarImage)
    {
        float currentFillAmount = healthBarImage.fillAmount;
        float targetFillAmount = target.currentHealth / target.maxHealth;
        float elapsedTime = 0f;
        float UIDelay = target.uiDelay;

        while (elapsedTime < UIDelay)
        {
            elapsedTime += Time.deltaTime;

            healthBarImage.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, elapsedTime / UIDelay);
            yield return null;
        }
        healthBarImage.fillAmount = targetFillAmount;
    }
}