using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsAudioManager : MonoBehaviour
{
    public static SoundEffectsAudioManager Instance;

    //[SerializeField] private AudioClip playButtonClip;
    [SerializeField] private AudioSource soundFXObject3D;
    [SerializeField] private AudioSource soundFXObject;
    
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButton()
    {
        //audioSource.clip = playButtonClip;
        audioSource.Play();
    }

    public void PlaySoundEffect(AudioSource myAudioSource, AudioClip audioClip)
    {
        myAudioSource.clip = audioClip;
        myAudioSource.Play();
    }

    public void PlaySoundEffectClipAtPoint(AudioClip audioClip, Transform spawnTransform, float volume, bool is3DAudio)
    {
        AudioSource myAudioSource;

        if (is3DAudio)
        {
            myAudioSource = Instantiate(soundFXObject3D, spawnTransform.position, Quaternion.identity);
        }
        else
        {
            myAudioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        }

        myAudioSource.clip = audioClip;

        myAudioSource.volume = volume;

        myAudioSource.Play();

        float clipLength = myAudioSource.clip.length;

        Destroy(myAudioSource.gameObject, clipLength);
    }
}
