using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlacksmithAudio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public enum AudioState { PAUSED, PLAYING, STOPPED };
        private AudioSource mainAudioSource;
        AudioState audioState = AudioState.STOPPED;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            DontDestroyOnLoad(instance);
            mainAudioSource = GetComponent<AudioSource>();
        }

        public void RandomlyPlayAudioClipFromList(AudioClip[] audioClips)
        {
            int randomClip = Random.Range(0, audioClips.Length);

            mainAudioSource.clip = audioClips[randomClip];
            PlayAudio();

        }

        public void StopAudio()
        {
            mainAudioSource.Stop();
            audioState = AudioState.STOPPED;
        }

        public void PlayAudio()
        {
            mainAudioSource.Play();
            audioState = AudioState.PLAYING;
        }

        public void ChangeAudioClip(AudioClip audioClip)
        {
            mainAudioSource.clip = audioClip;
        }

        public void PauseAudio()
        {
            mainAudioSource.Pause();
            audioState = AudioState.PAUSED;
        }

        
    }
}

