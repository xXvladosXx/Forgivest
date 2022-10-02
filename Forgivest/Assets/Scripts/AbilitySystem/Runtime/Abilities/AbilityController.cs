using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities
{
    [RequireComponent(typeof(GameplayEffectHandler),
        typeof(TagController))]
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private List<AbilityDefinition> _abilityDefinitions;
        public Dictionary<string, Ability> Abilities { get; protected set; } = new Dictionary<string, Ability>();
        public GameObject Target { get; set; }
        public ActiveAbility CurrentAbility { get; private set; }

        private GameplayEffectHandler _gameplayEffectHandler;
        private TagController _tagController;
        
        public event Action<ActiveAbility> OnAbilityActivated; 

        protected virtual void Awake()
        {
            _gameplayEffectHandler = GetComponent<GameplayEffectHandler>();
            _tagController = GetComponent<TagController>();
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
                    passiveAbility.ApplyEffects(gameObject);
                }
            }
        }

        public bool TryActiveAbility(string abilityName, GameObject target)
        {
            if (Abilities.TryGetValue(abilityName, out var ability))
            {
                if (ability is ActiveAbility activeAbility)
                {
                    if (!CanActivateAbility(activeAbility))
                    {
                        return false;
                    }
                    
                    Target = target;
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
                if (_tagController.ContainsAny(ability.ActiveAbilityDefinition.Cooldown.GrantedTags))
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
        
        private void CommitAbility(ActiveAbility ability)
        {
            _gameplayEffectHandler.ApplyGameplayEffectToSelf(
                new GameplayEffect(ability.ActiveAbilityDefinition.Cost, ability, gameObject));
            _gameplayEffectHandler.ApplyGameplayEffectToSelf(
                new GameplayPersistentEffect(ability.ActiveAbilityDefinition.Cooldown, ability, gameObject));
        }
    }
}