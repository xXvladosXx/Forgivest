using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active
{
    [AbilityType(typeof(AttackAbility))]
    [CreateAssetMenu (menuName = "AbilitySystem/Ability/AttackAbility")]
    public class AttackAbilityDefinition : ActiveAbilityDefinition
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float TimeToStop { get; private set; }
        [field: SerializeField] public float DistanceModifier { get; private set; } = 10;
    }
}