using System;
using CombatSystem.Scripts.Runtime.Core;
using Core;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using UnityEngine;
using Attribute = StatSystem.Attribute;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace MyGame
{
    [RequireComponent(typeof(StatController))]
    public class CombatableCharacter : MonoBehaviour, IDamageable
    {
        private const string k_Health = "Health";
        private bool m_IsInitialized;
        public float Health => (m_StatController.Stats[k_Health] as Attribute).CurrentValue;
        public float MaxHealth => m_StatController.Stats[k_Health].Value;
        public event Action OnHealthChanged;
        public event Action OnMaxHealthChanged;
        public bool IsInitialized => m_IsInitialized;
        public event Action OnInitialized;
        public event Action OnWillUninitialized;
        public event Action OnDefeated;
        public event Action<float> OnHealed;
        public event Action<float, bool> OnDamaged;

        protected StatController m_StatController;
        protected virtual void Awake()
        {
            m_StatController = GetComponent<StatController>();
        }

        protected virtual void OnEnable()
        {
            m_StatController.OnInitialized += OnStatControllerOnInitialized;
            m_StatController.WillUninitialize += OnStatControllerWillUninitialize;
            if (m_StatController.IsInitialized)
                OnStatControllerOnInitialized();
        }

        protected virtual void OnDisable()
        {
            m_StatController.OnInitialized -= OnStatControllerOnInitialized;
            m_StatController.WillUninitialize -= OnStatControllerWillUninitialize;
            if (m_StatController.IsInitialized)
                OnStatControllerWillUninitialize();
        }

        private void OnStatControllerWillUninitialize()
        {
            OnWillUninitialized?.Invoke();
            //m_StatController.Stats[k_Health].OnValueChanged -= MaxHealthChanged;
            //(m_StatController.Stats[k_Health] as Attribute).OnCurrentValueChanged -= HealthChanged;
            (m_StatController.Stats[k_Health] as Attribute).OnAppliedModifier -= OnAppliedModifier;
        }

        private void OnStatControllerOnInitialized()
        {
            //m_StatController.Stats[k_Health].OnValueChanged += MaxHealthChanged;
            //(m_StatController.Stats[k_Health] as Attribute).OnCurrentValueChanged += HealthChanged;
            (m_StatController.Stats[k_Health] as Attribute).OnAppliedModifier += OnAppliedModifier;
            m_IsInitialized = true;
            OnInitialized?.Invoke();
        }

        private void OnAppliedModifier(StatModifier modifier)
        {
            if (modifier.Magnitude > 0)
            {
                OnHealed?.Invoke(modifier.Magnitude);
            }
            else
            {
                if (modifier is HealthModifier healthModifier)
                {
                    OnDamaged?.Invoke(modifier.Magnitude, healthModifier.IsCriticalHit);
                }
                else
                {
                    OnDamaged?.Invoke(modifier.Magnitude, false);
                }
                if ((m_StatController.Stats[k_Health] as Attribute).CurrentValue == 0)
                    OnDefeated?.Invoke();
            }
        }

        private void HealthChanged()
        {
            OnHealthChanged?.Invoke();
        }

        private void MaxHealthChanged()
        {
            OnMaxHealthChanged?.Invoke();
        }

        public void TakeDamage(IDamage rawDamage)
        {
            (m_StatController.Stats[k_Health] as Attribute).ApplyModifier(new HealthModifier
            {
                Magnitude = rawDamage.Magnitude,
                Type = ModifierOperationType.Additive,
                Source = rawDamage.Source,
                IsCriticalHit = rawDamage.IsCriticalHit,
                Instigator = rawDamage.Instigator
            });
        }

        public void ApplyDamage(Object source, GameObject target)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            HealthModifier rawDamage = new HealthModifier
            {
                Instigator = gameObject,
                Type = ModifierOperationType.Additive,
                Magnitude = -1 * m_StatController.Stats["PhysicalAttack"].Value,
                Source = source,
                IsCriticalHit = false
            };

            if (m_StatController.Stats["CriticalHitChance"].Value / 100f >= Random.value)
            {
                rawDamage.Magnitude =
                    Mathf.RoundToInt(rawDamage.Magnitude * m_StatController.Stats["CriticalHitMultiplier"].Value /
                                     100f);
                rawDamage.IsCriticalHit = true;
            }
            
            damageable.TakeDamage(rawDamage);
        }
    }
}