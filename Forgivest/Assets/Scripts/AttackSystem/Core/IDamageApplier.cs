using System;
using InventorySystem.Items.Weapon;
using UnityEngine;

namespace AttackSystem.Core
{
    public interface IDamageApplier
    {
        Weapon Weapon { get; }
        event Action<AttackData> OnDamageApplied; 

        void ApplyDamage(AttackData attackData, float timeOfActivation);
    }
}