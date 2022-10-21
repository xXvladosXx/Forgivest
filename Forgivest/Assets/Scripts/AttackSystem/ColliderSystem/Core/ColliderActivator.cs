using System;
using UnityEngine;

namespace ColliderSystem.Core
{
    [RequireComponent(typeof(Collider))]
    public class ColliderActivator : MonoBehaviour
    {
        [field: SerializeField] public Collider Collider { get; private set; }

        private float _currentTime;
        
        public virtual void ActivateCollider(float time)
        {
            _currentTime = time;
            Collider.enabled = true;
        }

        public virtual void DeactivateCollider()
        {
            Collider.enabled = false;
        }
        
        protected void Update()
        {
            if(_currentTime == 0) return;

            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
            }
            else
            {
                DeactivateCollider();
            }
        }
    }
}