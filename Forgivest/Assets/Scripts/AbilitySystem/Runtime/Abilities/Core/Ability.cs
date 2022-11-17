using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AttackSystem.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Core
{
    public abstract class Ability
    {
        public AbilityDefinition AbilityDefinition { get; protected set; }
        protected AbilityHandler AbilityHandler;
        public AttackData AttackData { get; private set; }

        private int _level;
        public event Action OnLevelChanged;
        
        public int Level
        {
            get
            {
                if (_level == 0)
                {
                    _level = AbilityDefinition.Level;
                }
                
                return _level;
            } 
            set
            {
                int newLevel = Mathf.Min(value, AbilityDefinition.MaxLevel);
                if (newLevel != AbilityDefinition.Level && AbilityDefinition.Level > 0)
                {
                    _level = newLevel;
                    OnLevelChanged?.Invoke();
                }
            }
        }
        
        public Ability(AbilityDefinition definition, AbilityHandler abilityHandler)
        {
            AbilityHandler = abilityHandler;
            AbilityDefinition = definition;
            AbilityDefinition.User = abilityHandler.gameObject;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var gameplayEffectDefinition in AbilityDefinition.GameplayEffectDefinitions)
            {
                var attribute = gameplayEffectDefinition
                    .GetType()
                    .GetCustomAttributes(true)
                    .OfType<EffectTypeAttribute>()
                    .FirstOrDefault();
                
                var effect = Activator.CreateInstance(attribute.Type, gameplayEffectDefinition, this, AbilityHandler.gameObject, null) as GameplayEffect;
                
                stringBuilder.Append(effect).AppendLine();
            }
            
            return stringBuilder.ToString();
        }

        internal void ApplyEffects(GameObject other, AttackData attackData)
        {
            ApplyEffectsInternal(AbilityDefinition.GameplayEffectDefinitions, other, attackData);
        }

        private void ApplyEffectsInternal(ReadOnlyCollection<GameplayEffectDefinition> effectDefinitions,
            GameObject other, AttackData attackData)
        {
            if (other.TryGetComponent(out GameplayEffectHandler gameplayEffectController))
            {
                foreach (var effectDefinition in effectDefinitions)
                {
                    var attribute = effectDefinition
                        .GetType()
                        .GetCustomAttributes(true)
                        .OfType<EffectTypeAttribute>()
                        .FirstOrDefault();

                    var effect =
                        Activator.CreateInstance(attribute.Type, effectDefinition, this, AbilityHandler.gameObject, attackData) as GameplayEffect;
                    
                    gameplayEffectController.ApplyGameplayEffectToSelf(effect);
                }
            }
        }
    }
}