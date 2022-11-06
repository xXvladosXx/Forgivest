using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using AttackSystem.Core;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities
{
    public class PassiveAbility : Ability
    {
        public PassiveAbility(AbilityDefinition definition, AbilityHandler abilityHandler) : base(definition, abilityHandler)
        {
        }
    }
}