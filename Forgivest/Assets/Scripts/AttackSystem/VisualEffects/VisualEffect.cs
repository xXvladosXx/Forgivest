using System;
using System.Collections;
using UnityEngine;

namespace AttackSystem.VisualEffects
{
    public class VisualEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private Coroutine _coroutine;

        public event Action<VisualEffect> OnFinished;
        public bool IsLooping => _particleSystem.main.loop;
        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            if(_particleSystem == null)
                _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        public void Play()
        {
            _particleSystem.Play();
            if (!_particleSystem.main.loop)
                _coroutine = StartCoroutine(WaitForDuration());
        }

        public void Stop()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            
            _particleSystem.Stop();
            OnFinished?.Invoke(this);
        }

        private IEnumerator WaitForDuration()
        {
            yield return new WaitForSeconds(_particleSystem.main.duration);
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            yield return new WaitUntil(() => _particleSystem.particleCount == 0);
            OnFinished?.Invoke(this);
        }
    }
}