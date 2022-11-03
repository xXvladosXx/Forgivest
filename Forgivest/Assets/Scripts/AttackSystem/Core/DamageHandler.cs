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

        private bool _died;
        public event Action<AttackData> OnDied;

        public DamageHandler(StatController statController)
        {
            _statController = statController;
        }

        public void TakeDamage(AttackData attackData)
        {
            var taggable = attackData.DamageApplier as ITaggable ?? attackData.Weapon;

            var health = (_statController.Stats["Health"] as Health);
            health?.ApplyModifier(new HealthModifier
            {
                Magnitude = -attackData.Damage,
                Type = ModifierOperationType.Additive,
                Source = taggable,
                IsCriticalHit = attackData.IsCritical,
                Instigator = attackData.DamageReceiver.GameObject
            });

            if (health?.CurrentValue <= 0 && !_died)
            {
                attackData.DamageApplier.TakeRewards(attackData.DamageReceiver.Rewards);
                OnDied?.Invoke(attackData);
                _died = true;
            }
        }
    }
}