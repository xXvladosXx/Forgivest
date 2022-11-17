using System;
using AttackSystem.Core;
using ColliderSystem;
using InventorySystem;
using InventorySystem.Interaction;
using InventorySystem.Items.Weapon;
using UnityEngine;

namespace AttackSystem
{
    public class AttackApplier 
    {
        public void ApplyAttack(AttackData attackData, float timeOfActiveCollider, GameObject weapon)
        {
            Debug.Log(attackData.Damage);

            if(weapon.TryGetComponent(out AttackColliderActivator attackColliderActivator))
            {
                attackColliderActivator.ActivateCollider(attackData, timeOfActiveCollider);
            }
        }
    }
}