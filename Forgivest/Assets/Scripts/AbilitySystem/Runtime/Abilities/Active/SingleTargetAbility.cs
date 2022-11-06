using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AttackSystem.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active
{
    public class SingleTargetAbility : ActiveAbility
    {
        public SingleTargetAbility(SingleTargetAbilityDefinition definition, AbilityHandler abilityHandler) : base(definition, abilityHandler)
        {
        }

        public void Cast(GameObject target, AttackData attackData)
        {
            ApplyEffects(target, attackData);
        }
    }
}