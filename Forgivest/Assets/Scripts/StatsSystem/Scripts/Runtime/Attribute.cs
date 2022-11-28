using System;
using UnityEngine;

namespace StatsSystem.Scripts.Runtime
{
    public class Attribute : Stat
    {
        protected float m_CurrentValue;
        public float CurrentValue => m_CurrentValue;
        public event Action<float, float> OnCurrentValueChanged;
        public event Action<StatModifier> OnAppliedModifier;
        
        public Attribute(StatDefinition definition, StatController controller) : base(definition, controller)
        {
        }
        
        public override void Initialize()
        {
            base.Initialize();
            m_CurrentValue = Value;
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

            if (CurrentValue != newValue)
            {
                m_CurrentValue = newValue;
                OnCurrentValueChanged?.Invoke(m_CurrentValue, Value);
            }
            OnAppliedModifier?.Invoke(modifier);
        }
        
    }
}