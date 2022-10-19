using StatsSystem.Scripts.Runtime;
using StatSystem;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime
{
    public class GameplayEffectStatModifier : BaseGameplayEffectStatModifier
    {
        [field: SerializeField] public override string StatName { get; protected set; }
        [field: SerializeField] public override ModifierOperationType Type { get; protected set; }
    }
}