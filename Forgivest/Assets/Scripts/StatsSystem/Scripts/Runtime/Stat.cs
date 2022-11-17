using System;
using System.Collections.Generic;
using StatSystem;
using UnityEngine;

namespace StatsSystem.Scripts.Runtime
{
    public class Stat
    {
        protected StatDefinition m_Definition;
        protected StatController m_Controller;
        public StatDefinition Definition => m_Definition;
        protected float m_Value;
        public float Value => m_Value;
        public virtual float baseValue => m_Definition.BaseValue;
        public event Action<float> OnValueChanged;
        protected List<StatModifier> m_Modifiers = new List<StatModifier>();

        public Stat(StatDefinition definition, StatController controller)
        {
            m_Definition = definition;
            m_Controller = controller;
        }

        public virtual void Initialize()
        {
            CalculateOnValue(0);
        }

        public void AddModifier(StatModifier modifier)
        {
            m_Modifiers.Add(modifier);
            CalculateOnValue(0);
        }
        
        public void RemoveModifier(StatModifier modifier)
        {
            m_Modifiers.Remove(modifier);
            CalculateOnValue(0);
        }

        public void RemoveModifierFromSource(object source)
        {
            int num = m_Modifiers.RemoveAll(modifier => modifier.Source == source);
            if (num > 0)
            {
                CalculateOnValue(0);
            }
        }

        internal void CalculateOnValue(float maxValue)
        {
            float newValue = baseValue;

            if (m_Definition.Formula != null && m_Definition.Formula.rootNode != null)
            {
                newValue += Mathf.RoundToInt(m_Definition.Formula.rootNode.Value);
            }
            
            m_Modifiers.Sort((x, y) => x.Type.CompareTo(y.Type));

            for (int i = 0; i < m_Modifiers.Count; i++)
            {
                StatModifier modifier = m_Modifiers[i];
                if (modifier.Type == ModifierOperationType.Additive)
                {
                    newValue += modifier.Magnitude;
                }
                else if (modifier.Type == ModifierOperationType.Multiplicative)
                {
                    newValue *= modifier.Magnitude;
                }
            }
            
            if (m_Definition.Cap >= 0)
            {
                newValue = Mathf.Min(newValue, m_Definition.Cap);
            }

            if (m_Value != newValue)
            {
                m_Value = newValue;
                OnValueChanged?.Invoke(m_Value);
            }
        }
    }
}
