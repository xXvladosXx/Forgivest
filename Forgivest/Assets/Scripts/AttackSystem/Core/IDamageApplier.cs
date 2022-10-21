using System;
using ColliderSystem;
using CombatSystem.Scripts.Runtime;
using StatSystem.Scripts.Runtime;
using UnityEngine;
using Weapon = InventorySystem.Items.Weapon.Weapon;

namespace AttackSystem.Core
{
    public interface IDamageApplier
    {
        GameObject Weapon { get; }
        Weapon CurrentWeapon { get; }
        event Action<AttackData> OnDamageApplied;
        void ApplyAttack(float timeOfActivation);
        void ApplyShoot(Projectile projectile, Transform targetTransform, float definitionSpeed, ShotType definitionShotType, bool definitionIsSpin);
    }
}