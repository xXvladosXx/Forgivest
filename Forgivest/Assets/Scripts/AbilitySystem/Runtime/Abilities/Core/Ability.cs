using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Core
{
    public abstract class Ability
    {
        public AbilityDefinition AbilityDefinition { get; protected set; }
        protected AbilityController AbilityController;
        
        public Ability(AbilityDefinition definition, AbilityController abilityController)
        {
            AbilityController = abilityController;
            AbilityDefinition = definition;
        }

        internal void ApplyEffects(GameObject other)
        {
            ApplyEffectsInternal(AbilityDefinition.GameplayEffectDefinitions, other);
        }

        private void ApplyEffectsInternal(ReadOnlyCollection<GameplayEffectDefinition> effectDefinitions, GameObject other)
        {
            if (other.TryGetComponent(out GameplayEffectController gameplayEffectController))
            {
                foreach (var effectDefinition in effectDefinitions)
                {
                    var attribute = effectDefinition
                        .GetType()
                        .GetCustomAttributes(true)
                        .OfType<EffectTypeAttribute>()
                        .FirstOrDefault();

                    var effect =
                        Activator
                                .CreateInstance(attribute.Type, effectDefinition, this, AbilityController.gameObject)
                            as GameplayEffect;
                    
                    gameplayEffectController.ApplyGameplayEffectToSelf(effect);
                }
            }
        }
    }
}