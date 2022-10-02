using System;
using SaveSystem.Scripts.Runtime;
using UnityEngine;

namespace StatSystem
{
    public class Attribute : Stat, ISavable
    {
        protected int m_CurrentValue;
        public int currentValue => m_CurrentValue;
        public event Action currentValueChanged;
        public event Action<StatModifier> appliedModifier;
        
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
            int newValue = m_CurrentValue;
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
                currentValueChanged?.Invoke();
            }
            appliedModifier?.Invoke(modifier);
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
            currentValueChanged?.Invoke();
        }

        [Serializable]
        protected class AttributeData
        {
            public int currentValue;
        }

        #endregion
        
    }
}