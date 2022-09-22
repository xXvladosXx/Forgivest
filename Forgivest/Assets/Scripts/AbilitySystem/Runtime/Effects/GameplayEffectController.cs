using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AbilitySystem.AbilitySystem.Runtime;
using StatSystem;
using UnityEngine;
using Attribute = StatSystem.Attribute;

namespace AbilitySystem
{
    [RequireComponent(typeof(StatController),
        typeof(TagController))]
    public partial class GameplayEffectController : MonoBehaviour
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
            if (StatController.IsInitialized)
            {
                OnStatControllerInitialized();
            }
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

        public void ApplyGameplayEffectToSelf(GameplayEffect effectToApply)
        {
            if (effectToApply is GameplayPersistentEffect persistentEffect)
            {
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
        }

        private void AddActiveGameplayEffect(GameplayPersistentEffect persistentEffect)
        {
            ActiveEffects.Add(persistentEffect);
            AddSecondaryEffects(persistentEffect);
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
                if (StatController.stats.TryGetValue(((GameplayEffect) persistentEffect).Definition.Modifiers[i].StatName, out var stat))
                {
                    stat.AddModifier(persistentEffect.Modifiers[i]);
                }
            }

            foreach (var grantedTag in persistentEffect.Definition.GrantedTags)
            {
                TagController.AddTag(grantedTag);
            }

            if (persistentEffect.Definition.SpecialEffectDefinition != null)
            {
                PlaySpecialEffect(persistentEffect);
            }
        }

        private void RemoveSecondaryEffect(GameplayPersistentEffect persistentEffect)
        {
            foreach (var modifier in ((GameplayEffect) persistentEffect).Definition.Modifiers)
            {
                if (StatController.stats.TryGetValue(modifier.StatName, out var stat))
                {
                    stat.RemoveModifierFromSource(persistentEffect);
                }
            }
            
            foreach (var grantedTag in persistentEffect.Definition.GrantedTags)
            {
                TagController.RemoveTag(grantedTag);
            }

            if (persistentEffect.Definition.SpecialEffectDefinition != null)
            {
                StopSpecialEffect(persistentEffect);
            }
        }

        private void ExecuteGameplayEffect(GameplayEffect gameplayEffect)
        {
            for (int i = 0; i < gameplayEffect.Modifiers.Count; i++)
            {
                if (!StatController.stats
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
                if (!activeEffect.Definition.IsInfinite)
                {
                    activeEffect.RemainingDuration = Mathf.Max(activeEffect.RemainingDuration - Time.deltaTime, 0f);
                    if (Mathf.Approximately(activeEffect.RemainingDuration, 0f))
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
                if (StatController.stats.TryGetValue(modifier.StatName, out var stat))
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