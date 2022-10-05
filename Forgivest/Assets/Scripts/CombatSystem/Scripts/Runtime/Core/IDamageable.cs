using System;
using Core;

namespace CombatSystem.Scripts.Runtime.Core
{
    public interface IDamageable
    {
        float Health { get; }
        float MaxHealth { get; }
        event Action OnHealthChanged;
        event Action OnMaxHealthChanged;
        bool IsInitialized { get; }
        event Action OnInitialized;
        event Action OnWillUninitialized;
        event Action OnDefeated;
        event Action<float> OnHealed;
        event Action<float, bool> OnDamaged;
        void TakeDamage(IDamage rawDamage);
    }
}