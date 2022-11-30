using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    public class SoundManger : MonoBehaviour
    {
        [field: Header("AUDIO SOURCES")]
        [field: SerializeField] public AudioSource MusicSource { get; private set; }

        [SerializeField] private AudioSource _effectsSource;

        [Header("MUSIC CLIPS")] 
        [SerializeField] private AudioMixer _musicMixer;
        [SerializeField] private AudioClip _menuMusic;
        
        [SerializeField] private AudioClip _gameMusic;
        [SerializeField] private List<AudioClip> _effectsClips;
        [field: SerializeField] public List<AudioClip> GameMusicClips { get; private set; }

        private bool _canFade;
        private Coroutine _fadingCoroutine;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        
        public void SetEffectsSound(float value)
        {
            _effectsSource.volume = value;
        }
        public void SetMusicSound(float value)
        {
            MusicSource.volume = value;
        }
        
        public void PlayMusicSound(AudioClip audioClip)
        {
            MusicSource.loop = true;
            MusicSource.clip = audioClip;
            MusicSource.Play();
        }
        
        public void PlayEffectSound(AudioClip audioClip)
        {
            _effectsSource.PlayOneShot(audioClip);
        }

        public void StopPlayingMusic()
        {
            MusicSource.Stop();
        }

        public void PlayClip(AudioSource audioSource, List<AudioClip> list, int clipID, int nextClipID, bool loop = false)
        {
            PlayClipWithFade(audioSource, list, clipID);
            StartCoroutine(ChangeClipAfter(audioSource, list, nextClipID, list[clipID].length, loop));
        }

        public IEnumerator ChangeClipAfter(AudioSource _as, List<AudioClip> list, int clipID, float time,
            bool loop = false)
        {
            yield return new WaitForSecondsRealtime(time - 2f);

            if (loop)
            {
                PlayLoop(_as, list, clipID);
                yield break;
            }
            
            PlayClipWithFade(_as, list, clipID);
        }

        public void PlayLoop(AudioSource _as, List<AudioClip> list, int clipID = 0)
        {
            if (clipID + 1 < list.Count)
            {
                PlayClip(_as, list, clipID, clipID + 1, true);
            }
            else
            {
                PlayClip(_as, list, clipID, 0, true);
            }
        }

        public void PlayClipWithFade(AudioSource _as, List<AudioClip> list, int clipID, float fadeTime = 1.4f)
        {
            _canFade = true;

            _fadingCoroutine = StartCoroutine(FadeClip(_as, list, clipID, fadeTime));
        }

        public IEnumerator FadeClip(AudioSource _as, List<AudioClip> list, int clipID, float time)
        {
            if (!_canFade) yield break;

            _canFade = false;
            while (_as.volume > 0.1)
            {
                _as.volume -= Time.fixedDeltaTime * 2 / time;
                yield return new WaitForSecondsRealtime(0.001f);
            }

            _as.clip = list[clipID];
            _as.Play();

            while (_as.volume <= 0.97f)
            {
                _as.volume += Time.fixedDeltaTime * 2 / time;
                yield return new WaitForSecondsRealtime(0.001f);
            }

            _canFade = true;
        }
    }
}