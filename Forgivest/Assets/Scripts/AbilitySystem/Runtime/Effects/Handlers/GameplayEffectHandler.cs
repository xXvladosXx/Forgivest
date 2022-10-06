using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AbilitySystem.AbilitySystem.Runtime;
using AbilitySystem.AbilitySystem.Runtime.Effects.Stackable;
using StatSystem;
using UnityEngine;
using Attribute = StatSystem.Attribute;

namespace AbilitySystem
{
    [RequireComponent(typeof(StatController),
        typeof(TagController))]
    public partial class GameplayEffectHandler : MonoBehaviour
    {
        [SerializeField] private List<GameplayEffectDefinition> _startEffectDefinitions;

        
        protected StatController StatController;
        protected TagController TagController;
        protected List<GameplayPersistentEffect> ActiveEffects = new List<GameplayPersistentEffect>();
        public ReadOnlyCollection<GameplayPersistentEffect> GetActiveEffects => ActiveEffects.AsReadOnly();

        public bool IsInitialized { get; private set; }

        public event Action OnInitialized;

        private void Awake()
        {
            StatController = GetComponent<StatController>();
            TagController = GetComponent<TagController>();
        }

        private void OnEnable()
        {
            StatController.OnInitialized += OnStatControllerInitialized;
            TagController.OnTagAdded += CheckRemovalTagRequirements;
            TagController.OnTagRemoved += CheckRemovalTagRequirements;
            
            if (StatController.IsInitialized)
            {
                OnStatControllerInitialized();
            }
        }

        private void OnDisable()
        {
            StatController.OnInitialized -= OnStatControllerInitialized;
            TagController.OnTagAdded -= CheckRemovalTagRequirements;
            TagController.OnTagRemoved -= CheckRemovalTagRequirements;
        }

        private void OnStatControllerInitialized()
        {
            Init();
        }

        private void Init()
        {
            foreach (var startEffectDefinition in _startEffectDefinitions)
            {
                var attribute = startEffectDefinition
                    .GetType()
                    .GetCustomAttributes(true)
                    .OfType<EffectTypeAttribute>()
                    .FirstOrDefault();

                var effect =
                    Activator
                        .CreateInstance(attribute.Type, startEffectDefinition, _startEffectDefinitions,
                            gameObject) as GameplayEffect;

                ApplyGameplayEffectToSelf(effect);
            }

            IsInitialized = true;
            OnInitialized?.Invoke();
        }

        private void Update()
        {
            HandleDuration();
        }

        public bool ApplyGameplayEffectToSelf(GameplayEffect effectToApply)
        {
            foreach (var activeEffect in ActiveEffects)
            {
                foreach (var grantedImmunityTag in activeEffect.Definition.GrantedImmunityTags)
                {
                    if (effectToApply.Definition.Tags.Contains(grantedImmunityTag))
                    {
                        Debug.Log("You has immunity");
                        return false;
                    }
                }
            }

            if (!TagController.SatisfiesRequirements(effectToApply.Definition.ApplicationMustBePresentTags,
                    effectToApply.Definition.ApplicationMustBeAbsentTags))
            {
                Debug.Log("Not satisfies");
                return false;
            }
            
            if(effectToApply is GameplayPersistentEffect persistentEffectToApply)
            {
                if(!TagController.SatisfiesRequirements(persistentEffectToApply.Definition.PersistMustBePresentTags,
                    persistentEffectToApply.Definition.PersistMustBeAbsentTags))
                {
                    Debug.Log("Not satisfies");
                    return false;
                }
            }
            
            bool isAdded = true;
            
            if (effectToApply is GameplayStackableEffect stackableEffect)
            {
                var existingStackableEffect =
                    ActiveEffects.Find(effect => effect.Definition == stackableEffect.Definition) 
                        as GameplayStackableEffect;

                if (existingStackableEffect != null)
                {
                    isAdded = false;

                    if (existingStackableEffect.StackCount == existingStackableEffect.Definition.StackCountLimit)
                    {
                        foreach (var effectDefinition in existingStackableEffect.Definition.OverflowEffects)
                        {
                            var attribute = effectDefinition
                                .GetType()
                                .GetCustomAttributes(true)
                                .OfType<EffectTypeAttribute>()
                                .FirstOrDefault();

                            var overflowEffect = Activator
                                .CreateInstance(attribute.Type, effectDefinition,
                                existingStackableEffect, gameObject) as GameplayEffect;
                            
                            ApplyGameplayEffectToSelf(overflowEffect);
                        }

                        if (existingStackableEffect.Definition.ClearStackOnOverflow)
                        {
                            RemoveActiveGameplayEffect(existingStackableEffect, true);
                            isAdded = true;
                        }

                        if (existingStackableEffect.Definition.DenyOverflowApplication)
                        {
                            Debug.Log("Denied overflow application");
                            return false;
                        }
                    }

                    if (!isAdded)
                    {
                        existingStackableEffect.StackCount =
                            Math.Min(existingStackableEffect.StackCount + stackableEffect.StackCount,
                                existingStackableEffect.Definition.StackCountLimit);

                        if (existingStackableEffect.Definition.StackingDurationRefresh ==
                            GameplayEffectStackingDuration.RefreshOnSuccessfulApplication)
                        {
                            existingStackableEffect.RemainingDuration = existingStackableEffect.CurrentDuration;
                        }

                        if (existingStackableEffect.Definition.StackingPeriodReset ==
                            GameplayEffectStackingPeriod.ResetOnSuccessfulApplication)
                        {
                            existingStackableEffect.RemainingPeriod = existingStackableEffect.Definition.Period;
                        }
                    }
                }
            }

            foreach (var conditionalEffectDefinition in effectToApply.Definition.ApplicationMustBeAbsentTags)
            {
                var attribute = conditionalEffectDefinition
                    .GetType()
                    .GetCustomAttributes(true)
                    .OfType<EffectTypeAttribute>()
                    .FirstOrDefault();

                var conditionalEffect = Activator.CreateInstance(attribute.Type, 
                    conditionalEffectDefinition, effectToApply, effectToApply.Instigator) as GameplayEffect;

                ApplyGameplayEffectToSelf(conditionalEffect);
            }
            
            var effectsToRemove = new List<GameplayPersistentEffect>();
            foreach (var activeEffect in ActiveEffects)
            {
                foreach (var definitionTag in activeEffect.Definition.Tags)
                {
                    if (effectToApply.Definition.RemoveEffectsWithTags.Contains(definitionTag))
                    {
                        effectsToRemove.Add(activeEffect);
                    }
                }
            }

            foreach (var effect in effectsToRemove)
            {
                RemoveActiveGameplayEffect(effect, true);
            }

            if (effectToApply is GameplayPersistentEffect persistentEffect)
            {
                if(isAdded)
                    AddActiveGameplayEffect(persistentEffect);
            }
            else
            {
                ExecuteGameplayEffect(effectToApply);
            }

            if (effectToApply.Definition.SpecialEffectDefinition != null)
            {
                PlaySpecialEffect(effectToApply);
            }

            return true;
        }
        
        private void CheckRemovalTagRequirements(string tag)
        {
            ActiveEffects
                .Where(effect => !TagController
                .SatisfiesRequirements(effect.Definition.PersistMustBePresentTags, effect.Definition.PersistMustBeAbsentTags))
                .ToList()
                .ForEach(effect => RemoveActiveGameplayEffect(effect, true));
        }

        private void AddActiveGameplayEffect(GameplayPersistentEffect effect)
        {
            ActiveEffects.Add(effect);
            AddSecondaryEffects(effect);

            if (effect.Definition.IsPeriodic)
            {
                if (effect.Definition.IsExecutableEffectOnApply)
                {
                    ExecuteGameplayEffect(effect);
                }
            }
        }

        private void RemoveActiveGameplayEffect(GameplayPersistentEffect persistentEffect, bool prematureRemoval)
        {
            ActiveEffects.Remove(persistentEffect);
            RemoveSecondaryEffect(persistentEffect);
        }

        private void AddSecondaryEffects(GameplayPersistentEffect persistentEffect)
        {
            for (int i = 0; i < persistentEffect.Modifiers.Count; i++)
            {
                if (StatController.Stats.TryGetValue(((GameplayEffect) persistentEffect).Definition.Modifiers[i].StatName, out var stat))
                {
                    stat.AddModifier(persistentEffect.Modifiers[i]);
                }
            }

            foreach (var grantedTag in persistentEffect.Definition.GrantedTags)
            {
                TagController.AddTag(grantedTag);
            }

            if (persistentEffect.Definition.SpecialPersistentEffectDefinition != null)
            {
                PlaySpecialEffect(persistentEffect);
            }
        }

        private void RemoveSecondaryEffect(GameplayPersistentEffect persistentEffect)
        {
            foreach (var modifier in ((GameplayEffect) persistentEffect).Definition.Modifiers)
            {
                if (StatController.Stats.TryGetValue(modifier.StatName, out var stat))
                {
                    stat.RemoveModifierFromSource(persistentEffect);
                }
            }
            
            foreach (var grantedTag in persistentEffect.Definition.GrantedTags)
            {
                TagController.RemoveTag(grantedTag);
            }

            if (persistentEffect.Definition.SpecialPersistentEffectDefinition != null)
            {
                StopSpecialEffect(persistentEffect);
            }
        }

        private void ExecuteGameplayEffect(GameplayEffect gameplayEffect)
        {
            for (int i = 0; i < gameplayEffect.Modifiers.Count; i++)
            {
                if (!StatController.Stats
                        .TryGetValue(gameplayEffect.Definition.Modifiers[i].StatName,
                            out var stat)) continue;

                if (stat is Attribute attribute)
                {
                    attribute.ApplyModifier(gameplayEffect.Modifiers[i]);
                }
            }
        }

        private void HandleDuration()
        {
            List<GameplayPersistentEffect> effectsToRemove = new List<GameplayPersistentEffect>();
            foreach (var activeEffect in ActiveEffects)
            {
                if (activeEffect.Definition.IsPeriodic)
                {
                    activeEffect.RemainingPeriod = Math.Max(activeEffect.RemainingPeriod - Time.deltaTime, 0f);

                    if (Mathf.Approximately(activeEffect.RemainingPeriod, 0f))
                    {
                        ExecuteGameplayEffect(activeEffect);
                        activeEffect.RemainingPeriod = activeEffect.Definition.Period;
                    }
                }

                if (activeEffect.Definition.IsInfinite) continue;
                
                activeEffect.RemainingDuration = Mathf.Max(activeEffect.RemainingDuration - Time.deltaTime, 0f);
                if (Mathf.Approximately(activeEffect.RemainingDuration, 0f))
                {
                    if (activeEffect is GameplayStackableEffect stackableEffect)
                    {
                        switch (stackableEffect.Definition.StackingExpiration)
                        {
                            case GameplayEffectStackingExpiration.NeverRefresh:
                                effectsToRemove.Add(stackableEffect);
                                break;
                            case GameplayEffectStackingExpiration.RemoveSingleStackAndRefreshDuration:
                                
                                stackableEffect.StackCount--;
                                if (stackableEffect.StackCount == 0)
                                {
                                    effectsToRemove.Add(stackableEffect);
                                }
                                else
                                {
                                    activeEffect.RemainingDuration = activeEffect.CurrentDuration;
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        effectsToRemove.Add(activeEffect);
                    }
                }
            }

            foreach (var effect in effectsToRemove)
            {
                RemoveActiveGameplayEffect(effect, false);
            }
        }

        public bool CanApplyAttributeModifiers(GameplayEffectDefinition gameplayEffectDefinition)
        {
            foreach (var modifier in gameplayEffectDefinition.Modifiers)
            {
                if (StatController.Stats.TryGetValue(modifier.StatName, out var stat))
                {
                    if (stat is Attribute attribute)
                    {
                        if (modifier.Type == ModifierOperationType.Additive)
                        {
                            if (!(attribute.currentValue < Mathf.Abs(modifier.Formula.CalculateValue(gameObject))))
                                continue;

                            Debug.Log("Cannot satisfy costs");
                            return false;
                        }

                        Debug.Log("Only additional");
                        return false;
                    }

                    Debug.Log("Is not attribute");
                    return false;
                }

                Debug.Log("Was not found");
                return false;
            }

            return true;
        }
    }
}