using System;
using Core;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using Attribute = StatSystem.Attribute;

namespace AttackSystem.Core
{
    public class DamageHandler
    {
        private readonly StatController _statController;

        public event Action<AttackData> OnDied;

        public DamageHandler(StatController statController)
        {
            _statController = statController;
        }

        public void TakeDamage(AttackData attackData)
        {
            var taggable = attackData.DamageApplier as ITaggable ?? attackData.Weapon;

            (_statController.Stats["Health"] as Health)?.ApplyModifier(new HealthModifier
            {
                Magnitude = -attackData.Damage,
                Type = ModifierOperationType.Additive,
                Source = taggable,
                IsCriticalHit = attackData.IsCritical,
                Instigator = attackData.DamageReceiver.GameObject
            });

            if ((_statController.Stats["Health"] as Health)?.currentValue <= 0)
            {
                attackData.DamageApplier.TakeRewards(attackData.DamageReceiver.Rewards);
                OnDied?.Invoke(attackData);
            }
        }
    }
}