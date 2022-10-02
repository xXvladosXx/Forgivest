using System;
using CombatSystem.Scripts.Runtime.Core;
using Core;
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
        public int health => (m_StatController.stats[k_Health] as Attribute).currentValue;
        public int maxHealth => m_StatController.stats[k_Health].value;
        public event Action healthChanged;
        public event Action maxHealthChanged;
        public bool isInitialized => m_IsInitialized;
        public event Action initialized;
        public event Action willUninitialize;
        public event Action defeated;
        public event Action<int> healed;
        public event Action<int, bool> damaged;

        protected StatController m_StatController;
        protected virtual void Awake()
        {
            m_StatController = GetComponent<StatController>();
        }

        protected virtual void OnEnable()
        {
            m_StatController.OnInitialized += OnStatControllerOnInitialized;
            m_StatController.willUninitialize += OnStatControllerWillUninitialize;
            if (m_StatController.IsInitialized)
                OnStatControllerOnInitialized();
        }

        protected virtual void OnDisable()
        {
            m_StatController.OnInitialized -= OnStatControllerOnInitialized;
            m_StatController.willUninitialize -= OnStatControllerWillUninitialize;
            if (m_StatController.IsInitialized)
                OnStatControllerWillUninitialize();
        }

        private void OnStatControllerWillUninitialize()
        {
            willUninitialize?.Invoke();
            m_StatController.stats[k_Health].valueChanged -= OnMaxHealthChanged;
            (m_StatController.stats[k_Health] as Attribute).currentValueChanged -= OnHealthChanged;
            (m_StatController.stats[k_Health] as Attribute).appliedModifier -= OnAppliedModifier;
        }

        private void OnStatControllerOnInitialized()
        {
            m_StatController.stats[k_Health].valueChanged += OnMaxHealthChanged;
            (m_StatController.stats[k_Health] as Attribute).currentValueChanged += OnHealthChanged;
            (m_StatController.stats[k_Health] as Attribute).appliedModifier += OnAppliedModifier;
            m_IsInitialized = true;
            initialized?.Invoke();
        }

        private void OnAppliedModifier(StatModifier modifier)
        {
            if (modifier.Magnitude > 0)
            {
                healed?.Invoke(modifier.Magnitude);
            }
            else
            {
                if (modifier is HealthModifier healthModifier)
                {
                    damaged?.Invoke(modifier.Magnitude, healthModifier.IsCriticalHit);
                }
                else
                {
                    damaged?.Invoke(modifier.Magnitude, false);
                }
                if ((m_StatController.stats[k_Health] as Attribute).currentValue == 0)
                    defeated?.Invoke();
            }
        }

        private void OnHealthChanged()
        {
            healthChanged?.Invoke();
        }

        private void OnMaxHealthChanged()
        {
            maxHealthChanged?.Invoke();
        }

        public void TakeDamage(IDamage rawDamage)
        {
            (m_StatController.stats[k_Health] as Attribute).ApplyModifier(new HealthModifier
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
                Magnitude = -1 * m_StatController.stats["PhysicalAttack"].value,
                Source = source,
                IsCriticalHit = false
            };

            if (m_StatController.stats["CriticalHitChance"].value / 100f >= Random.value)
            {
                rawDamage.Magnitude =
                    Mathf.RoundToInt(rawDamage.Magnitude * m_StatController.stats["CriticalHitMultiplier"].value /
                                     100f);
                rawDamage.IsCriticalHit = true;
            }
            
            damageable.TakeDamage(rawDamage);
        }
    }
}