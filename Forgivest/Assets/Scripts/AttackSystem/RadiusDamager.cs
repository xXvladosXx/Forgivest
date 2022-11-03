using System;
using AbilitySystem.AbilitySystem.Runtime;
using AttackSystem.Core;
using UnityEngine;

namespace AttackSystem
{
    public class RadiusDamager : MonoBehaviour
    {
        [field: SerializeField] public VisualEffect VisualEffect { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }

        public event Action<CollisionData> OnHit;

        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleCollision(other);
        }

        private void HandleCollision(Collider other)
        {
            OnHit?.Invoke(new CollisionData
            {
                Source = this,
                Target = other.gameObject,
            });
        }
    }
}