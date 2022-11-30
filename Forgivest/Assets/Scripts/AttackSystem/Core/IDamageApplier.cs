using System;
using System.Collections.Generic;
using AttackSystem.Reward.Core;
using UnityEngine;
using Weapon = InventorySystem.Items.Weapon.Weapon;

namespace AttackSystem.Core
{
    public interface IDamageApplier
    {
        GameObject Weapon { get; }
        Weapon CurrentWeapon { get; }
        void ApplyAttack(float timeOfActivation);
        void ApplyShoot(Projectile projectile, Transform targetTransform, float definitionSpeed,
            ShotType definitionShotType, bool definitionIsSpin);
        void TakeRewards(List<IRewardable> damageReceiverRewards);
        event Action<AttackData> OnDamageApplied;
    }
}