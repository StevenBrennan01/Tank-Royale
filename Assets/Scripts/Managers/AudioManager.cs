using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {  get; private set; }

    public AudioSource menuAudio;
    public AudioSource levelAudio;

    //[SerializeField] private AudioSource[] shootAudio;

    private void Awake()
    {
        if (instance != null) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }
}