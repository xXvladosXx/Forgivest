using CombatSystem.Scripts.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Scripts.Runtime.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        private Slider m_Slider;
        private IDamageable m_Damageable;
        [SerializeField] private GameObject m_Owner;

        private void Awake()
        {
            m_Slider = GetComponent<Slider>();
            m_Damageable = m_Owner.GetComponent<IDamageable>();
        }

        private void OnEnable()
        {
            m_Damageable.OnInitialized += OnDamageableInitialized;
            m_Damageable.OnWillUninitialized += OnDamageableWillUninitialize;
            if (m_Damageable.IsInitialized)
                OnDamageableInitialized();
        }

        private void OnDamageableWillUninitialize()
        {
            UnregisterEvents();
        }

        private void OnDamageableInitialized()
        {
            m_Slider.maxValue = m_Damageable.MaxHealth;
            m_Slider.value = m_Damageable.Health;
            RegisterEvents();
        }

        private void UnregisterEvents()
        {
            m_Damageable.OnHealthChanged -= OnHealthChanged;
            m_Damageable.OnMaxHealthChanged -= OnMaxHealthChanged;
        }

        private void RegisterEvents()
        {
            m_Damageable.OnMaxHealthChanged += OnMaxHealthChanged;
            m_Damageable.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            m_Slider.value = m_Damageable.Health;
        }

        private void OnMaxHealthChanged()
        {
            m_Slider.maxValue = m_Damageable.MaxHealth;
        }

        private void OnValidate()
        {
            if (m_Owner != null)
            {
                if (m_Owner.GetComponent<IDamageable>() == null)
                {
                    Debug.LogWarning("The owner must implement the IDamageable interface!");
                    m_Owner = null;
                }
            }
        }
    }
}