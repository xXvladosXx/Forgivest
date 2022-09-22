using StatSystem;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime
{
    public class GameplayEffectStatModifyDamage : BaseGameplayEffectStatModifier
    {
        [field: SerializeField] public override string StatName { get; protected set; } = "Heath";

        [field: SerializeField]
        public override ModifierOperationType Type { get; protected set; } = ModifierOperationType.Additive;
        
        [field: SerializeField] public bool CanCauseCriticalHit { get; private set; }
    }
}