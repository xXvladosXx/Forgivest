using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active
{
    public class SingleTargetAbility : ActiveAbility
    {
        public SingleTargetAbility(SingleTargetAbilityDefinition definition, AbilityController abilityController) : base(definition, abilityController)
        {
        }

        public void Cast(GameObject target)
        {
            ApplyEffects(target);
        }
    }
}