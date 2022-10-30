using System;
using AttackSystem.Core;
using ColliderSystem.Core;
using UnityEngine;

namespace ColliderSystem
{
    public class AttackColliderActivator : ColliderActivator
    {
        private AttackData _currentAttackData;
        
        public void ActivateCollider(AttackData attackData, float time)
        {
            Collider.enabled = true;
            _currentTime = time;
            _currentAttackData = attackData;
        }

        private void OnCollisionEnter(Collision collision)
        {
            print("Entered collision");
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_currentAttackData == null) return;
            
            if (other.TryGetComponent(out IDamageReceiver damageReceiver))
            {
                if(damageReceiver.LayerMask == _currentAttackData.DamageApplierLayerMask) return;
                
                damageReceiver.ReceiveDamage(_currentAttackData);
            }
        }
    }
}