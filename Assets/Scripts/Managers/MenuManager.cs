using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //AudioManager audioManager_SCR;

    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject creditsUI;

    // Start is called before the first frame update
    private void Awake()
    {
        mainMenuUI.SetActive(true);
        creditsUI.SetActive(false);
        optionsUI.SetActive(false);

        //audioManager_SCR = FindObjectOfType<AudioManager>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        AudioManager.instance.menuAudio.Stop();
        //LAUNCH A LEVEL
        //FADE OUT MUSIC
    }

    public void QuitGame()
    {
        Application.Quit();
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

    public void ReturnToMenu()
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
}