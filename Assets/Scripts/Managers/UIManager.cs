using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private HealthManager healthManager_SCR;
    private ProjectileHandler projectileHandler_SCR;

    #region Inspector Header and Spacing
    [Header("-= Level UI =-")]
    [Space(15)]
    #endregion

    public GameObject healthUI;
    public GameObject pauseMenuUI;
    public GameObject reloadUI;

    [SerializeField] private GameObject[] bulletsUI;
    public int arrayIndex = 0;

    private float reloadRoundUIDelay = .5f;

    private Coroutine smoothHealthBar_CR;

    private void Awake()
    {
        if (Instance != null) { Destroy(this.gameObject); }
        else { Instance = this; }

        healthManager_SCR = FindObjectOfType<HealthManager>();
        projectileHandler_SCR = FindObjectOfType<ProjectileHandler>();

        reloadUI.SetActive(false);
    }

    public void DepleteAmmoUI()
    {
        StartCoroutine(DepleteAmmoUI_CR());
    }

    private IEnumerator DepleteAmmoUI_CR()
    {
        if (arrayIndex < bulletsUI.Length)
        {
            bulletsUI[arrayIndex].SetActive(false);
            arrayIndex++;
        }
        yield return null;
    }

    public void ReloadAmmoUI()
    {
        StartCoroutine(ReloadAmmoUI_CR());
    }

    public IEnumerator ReloadAmmoUI_CR()
    {
        int currentIndex = arrayIndex;

        for (int i = 0; i < currentIndex; i++)
        {
            if (arrayIndex > 0)
            {
                arrayIndex--;
                bulletsUI[arrayIndex].SetActive(true);
                yield return new WaitForSeconds(reloadRoundUIDelay);
            }
        }
    }

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