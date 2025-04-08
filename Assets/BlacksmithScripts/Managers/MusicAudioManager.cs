using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicAudioManager : MonoBehaviour
{
    public static MusicAudioManager instance;

    [SerializeField] private AudioClip menuMusicClip;
    [SerializeField] private AudioClip gameplayMusicClip;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.clip = menuMusicClip;
        audioSource.Play();
    }

    public void PlayGameplayMusic()
    {
        audioSource.clip = gameplayMusicClip;
        audioSource.Play();
    }
    
}
