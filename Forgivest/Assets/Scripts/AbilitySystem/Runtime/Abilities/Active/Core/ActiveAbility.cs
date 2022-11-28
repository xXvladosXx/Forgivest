using System.Text;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using AbilitySystem.AbilitySystem.Runtime.Effects.Core;
using AbilitySystem.AbilitySystem.Runtime.Effects.Persistent;
using AttackSystem.Core;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core
{
    public class ActiveAbility : Ability
    {
        public ActiveAbilityDefinition ActiveAbilityDefinition => AbilityDefinition as ActiveAbilityDefinition;
        public ActiveAbility(ActiveAbilityDefinition definition, AbilityHandler abilityHandler) : base(definition, abilityHandler)
        {
        }
        
        public override string ToString()
        {
            var stringBuilder = new StringBuilder(base.ToString());
            if (ActiveAbilityDefinition.Cost != null)
            {
                var cost = new GameplayEffect(ActiveAbilityDefinition.Cost, this, 
                    AbilityHandler.gameObject, AttackData);

                stringBuilder.Append(cost).AppendLine();
            }

            if (ActiveAbilityDefinition.Cooldown != null)
            {
                var cooldown = new GameplayPersistentEffect(ActiveAbilityDefinition.Cooldown, this, 
                    AbilityHandler.gameObject, AttackData);
                stringBuilder.Append(cooldown);
            }
            
            return stringBuilder.ToString();
        }
    }
}