using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {  get; private set; }

    //private AudioSource m_AudioSource;



    private void Awake()
    {
        if (instance != null) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    //public void StopMusic()
    //{
    //    audioSource.loop = false;
    //    StartCoroutine(FadeOutAndStop());
    //}

    //private IEnumerator FadeOutAndStop()
    //{
    //    float startVolume = audioSource.volume;

    //    for (float t = 0; t < audioFadeDuration; t += Time.deltaTime)
    //    {
    //        audioSource.volume = Mathf.Lerp(startVolume, 0, t / audioFadeDuration);
    //        yield return null;
    //    }

    //    audioSource.Stop();
    //}
}