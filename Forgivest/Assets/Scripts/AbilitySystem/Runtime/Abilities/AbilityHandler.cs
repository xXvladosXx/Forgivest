using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Passive;
using AbilitySystem.AbilitySystem.Runtime.Effects;
using AbilitySystem.AbilitySystem.Runtime.Effects.Core;
using AbilitySystem.AbilitySystem.Runtime.Effects.Persistent;
using InventorySystem;
using StatsSystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities
{
    [RequireComponent(typeof(GameplayEffectHandler),
        typeof(TagRegister))]
    public class AbilityHandler : MonoBehaviour
    {
        [field: SerializeField] public Inventory ItemContainer { get; private set; } 
        [field: SerializeField] public Inventory AllAbilities { get; private set; }
        [field: SerializeField] public Inventory Hotbar {get ; private set; }
        
        [SerializeField] private List<AbilityDefinition> _abilityDefinitions;
        public Dictionary<string, Ability> Abilities { get; protected set; } = new Dictionary<string, Ability>();
        public Dictionary<int, Ability> AbilitiesIndex { get; protected set; } = new Dictionary<int, Ability>();
        public ActiveAbility CurrentAbility { get; private set; }
        public int SkillPoints { get; private set; }

        private GameplayEffectHandler _gameplayEffectHandler;
        private TagRegister _tagRegister;
        public event Action<ActiveAbility> OnAbilityActivated; 
        public event Action<int> OnPointsChanged;

        protected virtual void Awake()
        {
            _gameplayEffectHandler = GetComponent<GameplayEffectHandler>();
            _tagRegister = GetComponent<TagRegister>();
        }

        private void OnEnable()
        {
            _gameplayEffectHandler.OnInitialized += OnEffectHandlerInit;
            if (_gameplayEffectHandler.IsInitialized)
            {
                OnEffectHandlerInit();
            }
        }

        private void OnDisable()
        {
            _gameplayEffectHandler.OnInitialized -= OnEffectHandlerInit;
        }

        private void OnEffectHandlerInit()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            int index = 0;
            foreach (var abilityDefinition in _abilityDefinitions)
            {
                var abilityAttributeType = abilityDefinition
                    .GetType()
                    .GetCustomAttributes(true)
                    .OfType<AbilityTypeAttribute>()
                    .FirstOrDefault();

                var ability = Activator.CreateInstance(abilityAttributeType.Type, abilityDefinition, this) as Ability;
                Abilities.Add(abilityDefinition.name, ability);

                if (ability is PassiveAbility passiveAbility)
                {
                    passiveAbility.ApplyEffects(gameObject, null);
                }
                
                AbilitiesIndex.Add(index, ability);
                index++;
            }
        }

        public bool TryActiveAbility(int abilityIndex)
        {
            if(Abilities.Count <= abilityIndex)
                return false;
            
            var abilityDefinition = Hotbar.ItemContainer.Slots[abilityIndex].Item.name;
            
            return TryActiveAbility(abilityDefinition);
        }
        public bool TryActiveAbility(string abilityName)
        {
            if (Abilities.TryGetValue(abilityName, out var ability))
            {
                if (ability is ActiveAbility activeAbility)
                {
                    if (!CanActivateAbility(activeAbility))
                    {
                        return false;
                    }
                    
                    CurrentAbility = activeAbility;
                    CommitAbility(activeAbility);
                    
                    OnAbilityActivated?.Invoke(activeAbility);

                    return true;
                } 
            }
            
            Debug.Log("Ability not found");
            return false;
        }

        public bool CanActivateAbility(ActiveAbility ability)
        {
            if (ability.ActiveAbilityDefinition.Cooldown != null)
            {
                if (_tagRegister.ContainsAny(ability.ActiveAbilityDefinition.Cooldown.GrantedTags))
                {
                    Debug.Log("Ability in cooldown");
                    return false;
                }
            }
            
            if (ability.ActiveAbilityDefinition.Cost != null)
            {
                return _gameplayEffectHandler.CanApplyAttributeModifiers(ability.ActiveAbilityDefinition.Cost);
            }

            return true;
        }


        public float GetCooldownOfAbility(string abilityName)
        {
            if (Abilities.TryGetValue(abilityName, out var ability))
            {
                if (ability is ActiveAbility activeAbility)
                {
                    if (activeAbility.ActiveAbilityDefinition.Cooldown != null)
                    {
                        return _tagRegister.GetDurationOfTag(activeAbility.ActiveAbilityDefinition.Cooldown.GrantedTags);
                    }
                }
            }

            return 1;
        }
        
        private void CommitAbility(ActiveAbility ability)
        {
            _gameplayEffectHandler.ApplyGameplayEffectToSelf(
                new GameplayEffect(ability.ActiveAbilityDefinition.Cost, ability, gameObject, ability.AttackData));
            _gameplayEffectHandler.ApplyGameplayEffectToSelf(
                new GameplayPersistentEffect(ability.ActiveAbilityDefinition.Cooldown, ability, gameObject, ability.AttackData));
        }


        public void AddPoints(int points)
        {
            SkillPoints += points;
            OnPointsChanged?.Invoke(SkillPoints);
        }
    }
}