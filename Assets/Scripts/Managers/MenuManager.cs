using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private int firstSceneIndex = 1;

    [Header("== UI Canvas ==")]
    [Space(10)]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject creditsUI;

    [Header("== Volume Settings ==")]
    [Space(10)]
    [SerializeField] private AudioMixer audioMixer;

    [Space(10)]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] TMP_Text masterVolValueToText;

    [Space(10)]
    [SerializeField] private Slider musicVolumeSlider = null;
    [SerializeField] TMP_Text musicVolValueToText;


    // Start is called before the first frame update
    private void Awake()
    {
        ActivateMenuUI();
    }

    #region Main Menu Management

    public void StartGame()
    {
        SceneManager.LoadScene(firstSceneIndex);

        //FADE OUT MUSIC
    }

    public void OptionsUI()
    {
        HideAllUI();
        optionsUI.SetActive(true);
    }

    public void CreditsUI()
    {
        HideAllUI();
        creditsUI.SetActive(true);
    }

    public void ActivateMenuUI()
    {
        HideAllUI();
        mainMenuUI.SetActive(true);
    }

    private void HideAllUI()
    {
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(false);
        creditsUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
    #endregion

    #region Options Menu Management

    public void SetMasterVolume(float masterVolumeValue)
    {
        AudioListener.volume = (masterVolumeValue);
        masterVolValueToText.text = masterVolumeValue.ToString("0.0");
    }

    public void SetMusicVolume(float musicVolumeValue)
    {

        musicVolValueToText.text = musicVolumeValue.ToString("0.0");
    }

    #endregion
}