using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //private HealthManager healthManager_SCR;
    private ProjectileHandler projectileHandler_SCR;

    //TankData
    [SerializeField]
    private TankAttributesSO tankData;

    #region Inspector Header and Spacing
    [Header("-= Level UI =-")]
    [Space(15)]
    #endregion

    public GameObject pauseMenuUI;

    public GameObject healthUI;
    public GameObject waveNumUI;
    public GameObject enemiesRemainingUI;
    public GameObject speedUI;
    public GameObject startOfLevelUI;
    public GameObject reloadUI;
    public GameObject emptyBulletsUI;
    public GameObject scoreUI;
    [SerializeField] private GameObject[] bulletsUI;
    [SerializeField] private GameObject[] livesUI;

    public GameObject bossIsSpawningUI;

    public GameObject NextWaveText;
    public GameObject NextWaveCountdown;
    private int countdownStartValue = 3;

    public int ammoIndex = 0;
    public int livesIndex = 0;

    private float reloadRoundUIDelay = .5f;

    private Coroutine smoothHealthBar_CR;

    [SerializeField] private Animator levelLoadAnim;

    private void OnEnable()
    {
        levelLoadAnim.SetTrigger("LevelLoad");

        NextWaveText.SetActive(false);
        NextWaveCountdown.SetActive(false);

        healthUI.SetActive(false);
        waveNumUI.SetActive(false);
        enemiesRemainingUI.SetActive(false);
        speedUI.SetActive(false);
        startOfLevelUI.SetActive(false);
        emptyBulletsUI.SetActive(false);
        scoreUI.SetActive(false);

        for (int i = 0; i < bulletsUI.Length; i++)
        {
            bulletsUI[i].SetActive(false);
        }

        for (int i = 0; i < livesUI.Length; i++)
        {
            livesUI[i].SetActive(false);
        }
    }

    public void StartLevelUI()
    {
        StartCoroutine(StartLevelUI_CR());
    }

    private IEnumerator StartLevelUI_CR()
    {
        startOfLevelUI.SetActive(true);

        yield return new WaitForSeconds(6f);

        SetAllUIActive();
    }

    private void SetAllUIActive()
    {
        healthUI.SetActive(true);
        waveNumUI.SetActive(true);
        enemiesRemainingUI.SetActive(true);
        scoreUI.SetActive(true);
        emptyBulletsUI.SetActive(true);

        for (int i = 0; i < bulletsUI.Length; i++)
        {
            bulletsUI[i].SetActive(true);
        }

        for (int i = 0; i < livesUI.Length; i++)
        {
            livesUI[i].SetActive(true);
        }

        startOfLevelUI.SetActive(false);
    }

    public void StartBossCountdown()
    {
        bossIsSpawningUI.SetActive(true);
        NextWaveCountdown.SetActive(true);

        StartCoroutine(CountdownTimer_CR());
        //StartCoroutine(BossCountdown_CR());
    }

    //private IEnumerator BossCountdown_CR()
    //{
    //    int currentCountdown = countdownStartValue;
    //    string waveStartText = "Go!";

    //    while (currentCountdown > 0)
    //    {
    //        NextWaveCountdown.GetComponent<TextMeshProUGUI>().text = currentCountdown.ToString();
    //        yield return new WaitForSeconds(1f);
    //        currentCountdown--;
    //    }
    //    bossIsSpawningUI.SetActive(false);

    //    NextWaveCountdown.GetComponent<TextMeshProUGUI>().text = waveStartText;
    //    yield return new WaitForSeconds(1f);
    //    NextWaveCountdown.SetActive(false);
    //}

    public void StartCountdownTimer()
    {
        NextWaveText.SetActive(true);
        NextWaveCountdown.SetActive(true);

        StartCoroutine(CountdownTimer_CR());
    }

    private IEnumerator CountdownTimer_CR()
    {
        int currentCountdown = countdownStartValue;
        string waveStartText = "Go!";

        while(currentCountdown > 0)
        {
            NextWaveCountdown.GetComponent<TextMeshProUGUI>().text = currentCountdown.ToString();
            yield return new WaitForSeconds(1f);
            currentCountdown--;
        }
        NextWaveText.SetActive(false);

        NextWaveCountdown.GetComponent<TextMeshProUGUI>().text = waveStartText;
        yield return new WaitForSeconds(1f);
        NextWaveCountdown.SetActive(false);
    }

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(this.gameObject); }

        //healthManager_SCR = FindObjectOfType<HealthManager>();
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

    private IEnumerator ReloadAmmoUI_CR()
    {
        int currentIndex = ammoIndex;

        for (int i = 0; i < currentIndex; i++)
        {
            if (ammoIndex > 0)
            {
                ammoIndex--;
                bulletsUI[ammoIndex].SetActive(true);
                yield return new WaitForSeconds(reloadRoundUIDelay);
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

    public void IncreaseLives()
    {
        StartCoroutine(IncreaseLives_CR());
    }

    public IEnumerator IncreaseLives_CR()
    {
        int currentIndex = livesIndex;

        for (int i = 0; i < currentIndex; i++)
        {
            if (livesIndex > 0)
            {
                livesIndex--;
                livesUI[livesIndex].SetActive(true);
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

    public void SpeedBarDepletion()
    {
        speedUI.SetActive(true);
        StartCoroutine(SpeedBarDepletion_CR());
    }

    private IEnumerator SpeedBarDepletion_CR()
    {
        // Assuming you have a speed bar Image component
        Image speedBarImage = speedUI.GetComponentInChildren<Image>();
        float currentFillAmount = speedBarImage.fillAmount;
        float targetFillAmount = 0f; // Deplete to empty
        float elapsedTime = 0f;
        float depletionDuration = 3f; // Duration to deplete the speed bar

        while (elapsedTime < depletionDuration)
        {
            elapsedTime += Time.deltaTime;
            speedBarImage.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, elapsedTime / depletionDuration);
            yield return null;
        }
        speedBarImage.fillAmount = targetFillAmount;

        speedUI.SetActive(false); // Hide the speed bar after depletion
        speedBarImage.fillAmount = 1f; // Reset the speed bar fill amount for next use
    }
}