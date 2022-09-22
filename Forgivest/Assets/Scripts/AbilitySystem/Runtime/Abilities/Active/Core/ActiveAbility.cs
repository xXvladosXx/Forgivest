using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core
{
    public class ActiveAbility : Ability
    {
        public ActiveAbilityDefinition ActiveAbilityDefinition => AbilityDefinition as ActiveAbilityDefinition;
        public ActiveAbility(ActiveAbilityDefinition definition, AbilityController abilityController) : base(definition, abilityController)
        {
        }
    }
}