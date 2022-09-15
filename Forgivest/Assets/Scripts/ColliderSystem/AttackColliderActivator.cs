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
            _currentAttackData = attackData;
            ActivateCollider(time);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageReceiver damageReceiver))
            {
                if(damageReceiver.LayerMask == _currentAttackData.DamageApplierLayerMask) return;
                
                damageReceiver.ReceiverDamage(_currentAttackData);
            }
        }
    }
}