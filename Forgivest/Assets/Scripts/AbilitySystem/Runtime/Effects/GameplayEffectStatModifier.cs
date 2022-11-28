using AbilitySystem.AbilitySystem.Runtime.Effects.Core;
using StatsSystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Effects
{
    public class GameplayEffectStatModifier : BaseGameplayEffectStatModifier
    {
        [field: SerializeField] public override string StatName { get; protected set; }
        [field: SerializeField] public override ModifierOperationType Type { get; protected set; }
    }
}