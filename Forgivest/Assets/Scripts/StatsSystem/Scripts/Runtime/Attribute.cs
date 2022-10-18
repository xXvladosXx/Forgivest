using System;
using SaveSystem.Scripts.Runtime;
using StatsSystem.Scripts.Runtime;
using UnityEngine;

namespace StatSystem
{
    public class Attribute : Stat, ISavable
    {
        protected float m_CurrentValue;
        public float currentValue => m_CurrentValue;
        public event Action OnCurrentValueChanged;
        public event Action<StatModifier> OnAppliedModifier;
        
        public Attribute(StatDefinition definition, StatController controller) : base(definition, controller)
        {
        }
        
        public override void Initialize()
        {
            base.Initialize();
            m_CurrentValue = value;
        }

        public virtual void ApplyModifier(StatModifier modifier)
        {
            var newValue = m_CurrentValue;
            switch (modifier.Type)
            {
                case ModifierOperationType.Override:
                    newValue = modifier.Magnitude;
                    break;
                case ModifierOperationType.Additive:
                    newValue += modifier.Magnitude;
                    break;
                case ModifierOperationType.Multiplicative:
                    newValue *= modifier.Magnitude;
                    break;
            }

            newValue = Mathf.Clamp(newValue, 0, m_Value);

            if (currentValue != newValue)
            {
                m_CurrentValue = newValue;
                OnCurrentValueChanged?.Invoke();
            }
            OnAppliedModifier?.Invoke(modifier);
        }

        #region Save System

        public object Data => new AttributeData
        {
            currentValue = currentValue
        };
        public void Load(object data)
        {
            AttributeData attributeData = (AttributeData)data;
            m_CurrentValue = attributeData.currentValue;
            OnCurrentValueChanged?.Invoke();
        }

        [Serializable]
        protected class AttributeData
        {
            public float currentValue;
        }

        #endregion
        
    }
}