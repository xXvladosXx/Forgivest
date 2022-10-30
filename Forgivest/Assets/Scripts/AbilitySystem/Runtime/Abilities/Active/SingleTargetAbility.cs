using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AttackSystem.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active
{
    public class SingleTargetAbility : ActiveAbility
    {
        public SingleTargetAbility(SingleTargetAbilityDefinition definition, AbilityController abilityController, AttackData attackData) : base(definition, abilityController, attackData)
        {
        }

        public void Cast(GameObject target)
        {
            ApplyEffects(target);
        }
    }
}