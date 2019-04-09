using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class AudioEffectManager : MonoBehaviour
    {
        private Dictionary<string, AudioSource> _audioSources;

        private void Start()
        {
            _audioSources = new Dictionary<string, AudioSource>();
            var audioSoures = GetComponentsInChildren<AudioSource>();
            foreach (var audioSource in audioSoures)
            {
                _audioSources.Add(audioSource.name, audioSource);
            }
        }

        public void PlayAudio(string name)
        {
            if (!_audioSources.ContainsKey(name)) return;

            var audio = _audioSources[name];
            
            audio.Play();
        }
        
        public void StopAudio(string name)
        {
            if (!_audioSources.ContainsKey(name)) return;

            var audio = _audioSources[name];
            
            audio.Stop();
        }

        public void PlayAudioFadeIn(string name, float targetVolume, float fadeTime)
        {
            if (!_audioSources.ContainsKey(name)) return;

            var audio = _audioSources[name];
            StartCoroutine(FadeIn(audio, targetVolume, fadeTime));
        }
        
        public void PlayAudioFadeOut(string name, float fadeTime)
        {
            if (!_audioSources.ContainsKey(name)) return;

            var audio = _audioSources[name];
            StartCoroutine(FadeOut(audio, fadeTime));
        }
        
        public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime)
        {
            var startVolume = audioSource.volume; 
 
            while (audioSource.volume > 0) {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
                yield return null;
            }
 
            audioSource.Stop ();
            audioSource.volume = startVolume;
        }
        
        public static IEnumerator FadeIn (AudioSource audioSource, float targetVolume, float FadeTime)
        {
            audioSource.volume = 0;
            audioSource.Play();
            
            while (audioSource.volume < targetVolume) {
                audioSource.volume += targetVolume * Time.deltaTime / FadeTime;
 
                yield return null;
            }
        }
        
        #region singleton
        private static AudioEffectManager _instance;

        public static AudioEffectManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AudioEffectManager>();
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }
        #endregion
    }
}