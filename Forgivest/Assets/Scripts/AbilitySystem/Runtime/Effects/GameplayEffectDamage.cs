using StatSystem;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime
{
    public class GameplayEffectDamage : BaseGameplayEffectStatModifier
    {
        [field: SerializeField] public override string StatName { get; protected set; } = "Health";

        [field: SerializeField]
        public override ModifierOperationType Type { get; protected set; } = ModifierOperationType.Additive;
        
        [field: SerializeField] public bool CanCauseCriticalHit { get; private set; }
    }
}