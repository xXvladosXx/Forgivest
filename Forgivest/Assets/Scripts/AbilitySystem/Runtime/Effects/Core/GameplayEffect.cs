using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AttackSystem.Core;
using Core;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AbilitySystem.AbilitySystem.Runtime
{
    public class GameplayEffect : ITaggable
    {
        [field: SerializeField] public GameplayEffectDefinition Definition { get; protected set; }
        public object Source { get; private set; }
        public GameObject Instigator { get; private set; }
        public AttackData AttackData { get; }

        private List<StatModifier> _modifiers = new List<StatModifier>();
        public ReadOnlyCollection<StatModifier> Modifiers => _modifiers.AsReadOnly();

        public ReadOnlyCollection<string> Tags => Definition.Tags;
        
        public GameplayEffect(
            GameplayEffectDefinition definition,
            object source,
            GameObject instigator,
            AttackData attackData)
        {
            Definition = definition;
            Source = source;
            Instigator = instigator;
            AttackData = attackData;

            var statController = instigator.GetComponent<StatController>();
            foreach (var modifier in definition.Modifiers)
            {
                StatModifier statModifier;
                
                if (modifier is GameplayEffectDamage effectDamage)
                {
                    HealthModifier healthModifier = new HealthModifier
                    {
                        Magnitude = Mathf.RoundToInt(modifier.Formula.CalculateValue(instigator)),
                        IsCriticalHit = false
                    };

                    if (effectDamage.CanCauseCriticalHit)
                    {
                        if (statController.Stats["CriticalHitChance"].Value / 100f >= Random.value)
                        {
                            healthModifier.Magnitude = Mathf.RoundToInt(healthModifier.Magnitude *
                                statController.Stats["CriticalHitMultiplier"].Value / 100f);
                            healthModifier.IsCriticalHit = true;
                        }
                    }

                    statModifier = healthModifier;
                }
                else
                {
                    statModifier = new StatModifier
                    {
                        Magnitude = Mathf.RoundToInt(modifier.Formula.CalculateValue(instigator))
                    };
                }

                statModifier.Source = this;
                statModifier.Type = modifier.Type;
                _modifiers.Add(statModifier);
            }
        }

        public override string ToString()
        {
            return ReplaceMacro(Definition.Description, this);
        }
        
        protected string ReplaceMacro(string value, object @object)
        {
            if(value == null) return "";
            
            return Regex.Replace(value, @"{(.+?)}", match =>
            {
                var p = Expression.Parameter(@object.GetType(), @object.GetType().Name);
                var e = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(new[] { p }, null,
                    match.Groups[1].Value);
                return (e.Compile().DynamicInvoke(@object) ?? "").ToString();
            });
        }
    }
}

/*
var regex = new Regex(@"{.+?}", RegexOptions.Compiled);
            var matches = regex.Matches(value);

            foreach (Match match in matches)
            {
                if(!match.Success) continue;
                
                var matchedValue = match.ToString();

                if (matchedValue.Length <= 2)
                {
                    continue;
                }

                matchedValue = matchedValue[1..^1];

                var indexerRegex = new Regex(@"\[\d+\]", RegexOptions.Compiled);
                
                var propsOrFieldsOrMethodsOrIndexers = matchedValue
                    .Split('.')
                    .SelectMany(x => 
                        indexerRegex.IsMatch(x) ? 
                            new[] { x.Remove(x.IndexOf('[')), indexerRegex.Match(x).Value } : 
                            new[] { x })
                    .ToArray();
                        
                var currentObject = @object;

                foreach (var propOrFieldOrMethodOrIndexer in propsOrFieldsOrMethodsOrIndexers)
                {
                    if (currentObject == null)
                    {
                        throw new ArgumentNullException(nameof(@object));
                    }

                    var type = currentObject.GetType();
                    var prop = type.GetProperty(propOrFieldOrMethodOrIndexer);
                    var field = type.GetField(propOrFieldOrMethodOrIndexer);
                    var indexer = type.GetProperties().FirstOrDefault(x => x.GetIndexParameters().Length > 0);
                    var method = type.GetMethod(propOrFieldOrMethodOrIndexer);

                    if (prop != null)
                    {
                        currentObject = prop.GetValue(currentObject);
                    }
                    else if (field != null)
                    {
                        currentObject = field.GetValue(currentObject);
                    }
                    else if (method != null)
                    {
                        currentObject = method.Invoke(currentObject, null);
                    }
                    else if (indexer != null && propOrFieldOrMethodOrIndexer.StartsWith('[') &&
                             propOrFieldOrMethodOrIndexer.EndsWith(']'))
                    {
                        currentObject = indexer.GetValue(currentObject,
                            new object[] {int.Parse(propOrFieldOrMethodOrIndexer[1..^1])});
                    }
                    else
                    {
                        throw new ArgumentException(matchedValue);
                    }
                }

                var replacement = match.Result(currentObject.ToString());
                value = value.Replace(matchedValue, replacement);
            }

            return value.Replace("{", "").Replace("}", "");
            */