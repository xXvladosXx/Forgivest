using System;
using System.Collections.ObjectModel;
using System.Linq;
using AttackSystem.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Core
{
    public abstract class Ability
    {
        public AbilityDefinition AbilityDefinition { get; protected set; }
        protected AbilityController AbilityController;
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
        
        public Ability(AbilityDefinition definition, AbilityController abilityController, AttackData attackData)
        {
            AbilityController = abilityController;
            AbilityDefinition = definition;
            AttackData = attackData;
        }

        internal void ApplyEffects(GameObject other)
        {
            ApplyEffectsInternal(AbilityDefinition.GameplayEffectDefinitions, other);
        }

        private void ApplyEffectsInternal(ReadOnlyCollection<GameplayEffectDefinition> effectDefinitions, GameObject other)
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
                        Activator.CreateInstance(attribute.Type, effectDefinition, this, AbilityController.gameObject, AttackData) as GameplayEffect;
                    
                    gameplayEffectController.ApplyGameplayEffectToSelf(effect);
                }
            }
        }
    }
}