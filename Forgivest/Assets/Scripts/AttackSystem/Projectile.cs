using System;
using AbilitySystem.AbilitySystem.Runtime;
using AttackSystem.Core;
using AttackSystem.VisualEffects;
using UnityEngine;

namespace AttackSystem
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private VisualEffect _visualEffect;

        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }
        
        public event Action<CollisionData> OnHit;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider collider)
        {
            HandleCollision(collider.gameObject);
        }

        protected void HandleCollision(GameObject target)
        {
            if(target.layer == gameObject.layer) return;
            if(_visualEffect != null)
            {
                 var collisionVisualEffect = Instantiate(_visualEffect, transform.position, transform.rotation);
                 collisionVisualEffect.OnFinished += e => Destroy(collisionVisualEffect.gameObject);
                 collisionVisualEffect.Play();
            }
            
            OnHit?.Invoke(new CollisionData
            {
                Source = this,
                Target = target
            });
        }
    }
}