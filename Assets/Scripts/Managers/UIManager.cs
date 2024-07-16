using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private HealthManager healthManager_SCR;

    #region Inspector Header and Spacing
    [Header("-= UI Manager =-")]
    [Space(15)]
    #endregion

    public GameObject healthUI;
    public GameObject pauseMenuUI;

    private Coroutine smoothHealthBar_CR;

    private void Awake()
    {
        healthManager_SCR = FindObjectOfType<HealthManager>();
    }

    public void UpdateHealthUI(HealthManager target, Image healthBarImage)
    {
        smoothHealthBar_CR = StartCoroutine(SmoothHealthBar(target, healthBarImage));
    }

    //SLOWING DOWN HEALTHBAR UPDATE

    //LOOK AT COLOR.LERP FOR HEALTHBAR COLOUR CHANGE?
    private IEnumerator SmoothHealthBar(HealthManager target, Image healthBarImage)
    {
        float currentFillAmount = healthBarImage.fillAmount;
        float targetFillAmount = target.currentHealth / target.maxHealth;
        float elapsedTime = 0f;
        float UIDelay = target.UIDelay;

        while (elapsedTime < UIDelay)
        {
            elapsedTime += Time.deltaTime;

            healthBarImage.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, elapsedTime / UIDelay);
            yield return null;
        }
        healthBarImage.fillAmount = targetFillAmount;
    }
}