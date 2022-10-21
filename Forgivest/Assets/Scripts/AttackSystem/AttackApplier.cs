/*using System;
using AttackSystem.Core;
using InventorySystem;
using InventorySystem.Interaction;
using InventorySystem.Items.Weapon;
using UnityEngine;

namespace AttackSystem
{
    public class AttackApplier : IDamageApplier
    {
        private readonly ItemEquipHandler _itemEquipHandler;

        public Weapon CurrentWeapon => _itemEquipHandler.CurrentWeapon;
        public AttackApplier(ItemEquipHandler itemEquipHandler)
        {
            _itemEquipHandler = itemEquipHandler;
        }

        public event Action<AttackData> OnDamageApplied;
        
        //public 

        public void ApplyDamage(AttackData attackData, float timeOfActivation)
        {
            Debug.Log(attackData.Damage);
            
            //_itemEquipHandler.CurrentColliderWeapon.ActivateCollider(attackData, timeOfActivation);
        }
    }
}*/