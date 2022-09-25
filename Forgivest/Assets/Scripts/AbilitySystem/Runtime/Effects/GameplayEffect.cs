using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core;
using StatSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AbilitySystem.AbilitySystem.Runtime
{
    [Serializable]
    public class GameplayEffect : ITaggable
    {
        [field: SerializeField] public GameplayEffectDefinition Definition { get; protected set; }
        public object Source { get; private set; }
        public GameObject Instigator { get; private set; }

        private List<StatModifier> _modifiers = new List<StatModifier>();
        public ReadOnlyCollection<StatModifier> Modifiers => _modifiers.AsReadOnly();

        public ReadOnlyCollection<string> Tags => Definition.Tags;
        
        public GameplayEffect(
            GameplayEffectDefinition definition,
            object source,
            GameObject instigator)
        {
            Definition = definition;
            Source = source;
            Instigator = instigator;

            var statController = instigator.GetComponent<StatController>();
            foreach (var modifier in definition.Modifiers)
            {
                StatModifier statModifier;
                
                if (modifier is GameplayEffectDamage effectDamage)
                {
                    HealthModifier healthModifier = new HealthModifier
                    {
                        magnitude = Mathf.RoundToInt(modifier.Formula.CalculateValue(instigator)),
                        isCriticalHit = false
                    };

                    if (effectDamage.CanCauseCriticalHit)
                    {
                        if (statController.stats["CriticalHitChance"].value / 100f >= Random.value)
                        {
                            healthModifier.magnitude = Mathf.RoundToInt(healthModifier.magnitude *
                                statController.stats["CriticalHitMultiplier"].value / 100f);
                            healthModifier.isCriticalHit = true;
                        }
                    }

                    statModifier = healthModifier;
                }
                else
                {
                    statModifier = new StatModifier
                    {
                        magnitude = Mathf.RoundToInt(modifier.Formula.CalculateValue(instigator))
                    };
                }

                statModifier.source = this;
                statModifier.type = modifier.Type;
                _modifiers.Add(statModifier);
            }
        }

    }
}