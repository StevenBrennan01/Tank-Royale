using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private int firstSceneIndex = 1;

    [SerializeField] private Animator transitionFadeAnim;
    //[SerializeField] private Animator transitionFadeOutAnim;

    private Coroutine LoadLevel;
    //private Coroutine RunFade;

    public void LoadMap()
    {
        LoadLevel = StartCoroutine(LoadLevel_CR());
    }

    //public void RunFade()
    //{
    //    RunFade = StartCoroutine(RunFade_CR());
    //}

    IEnumerator LoadLevel_CR()
    {
        if (transitionFadeAnim != null)
        {
            transitionFadeAnim.SetTrigger("LevelLoading");
            yield return new WaitForSeconds(3);

            SceneManager.LoadScene(firstSceneIndex);
        }
    }

    IEnumerator RunFade_CR()
    {
        if (transitionFadeAnim != null)
        {
            transitionFadeAnim.SetTrigger("LevelLoading");
            yield return new WaitForSeconds(1);
        }
    }

    public void EnableAnimator()
    {
        transitionFadeAnim.enabled = true;
    }

    public void DisableAnimator()
    {
        transitionFadeAnim.enabled = false;
    }
}
