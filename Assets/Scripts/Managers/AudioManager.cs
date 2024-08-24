using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {  get; private set; }

    private AudioSource m_audioSource;

    private float audioFadeDuration = 2f;
    private float targetVolume = 3f;

    private void Awake()
    {
        if (instance != null) { Destroy(this.gameObject); }
        else { instance = this; }

        m_audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void StopMusic()
    {
        m_audioSource.loop = false;
        StartCoroutine(FadeOutAndStop());
    }

    private IEnumerator FadeOutAndStop()
    {
        float currentVolume = m_audioSource.volume;

        for (float t = 0; t < audioFadeDuration; t += Time.deltaTime)
        {
            m_audioSource.volume = Mathf.Lerp(currentVolume, 0, t / audioFadeDuration);
            yield return null;
        }

        m_audioSource.Stop();
    }
}