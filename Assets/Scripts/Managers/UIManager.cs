using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject[] livesUI;

    public int ammoIndex = 0;
    public int livesIndex = 0;

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
        if (ammoIndex < bulletsUI.Length)
        {
            bulletsUI[ammoIndex].SetActive(false);
            ammoIndex++;
        }
        yield return null;
    }

    public void ReloadAmmoUI()
    {
        StartCoroutine(ReloadAmmoUI_CR());
    }

    public IEnumerator ReloadAmmoUI_CR()
    {
        int currentIndex = ammoIndex;

        for (int i = 0; i < currentIndex; i++)
        {
            if (ammoIndex > 0)
            {
                ammoIndex--;
                bulletsUI[ammoIndex].SetActive(true);
                yield return null;
            }
        }
    }

    public void DepleteLives()
    {
        StartCoroutine(DepleteLives_CR());
    }

    private IEnumerator DepleteLives_CR()
    {
        if (livesIndex <= livesUI.Length)
        {
            livesUI[livesIndex].SetActive(false);
            livesIndex++;
        }
        yield return null;
    }

    //public IEnumerator IncreaseLives_CR()
    //{
    //    int currentIndex = livesIndex;

    //    for (int i = 0; i < currentIndex; i++)
    //    {
    //        if (livesIndex > 0)
    //        {
    //            livesIndex--;
    //            livesUI[livesIndex].SetActive(true);
    //            yield return null;
    //        }
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