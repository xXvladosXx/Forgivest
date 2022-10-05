using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StatSystem
{
    public class Stat
    {
        protected StatDefinition m_Definition;
        protected StatController m_Controller;
        public StatDefinition definition => m_Definition;
        protected float m_Value;
        public float value => m_Value;
        public virtual float baseValue => m_Definition.BaseValue;
        public event Action valueChanged;
        protected List<StatModifier> m_Modifiers = new List<StatModifier>();

        public Stat(StatDefinition definition, StatController controller)
        {
            m_Definition = definition;
            m_Controller = controller;
        }

        public virtual void Initialize()
        {
            CalculateValue();
        }

        public void AddModifier(StatModifier modifier)
        {
            m_Modifiers.Add(modifier);
            CalculateValue();
        }

        public void RemoveModifierFromSource(object source)
        {
            int num = m_Modifiers.RemoveAll(modifier => modifier.Source == source);
            if (num > 0)
            {
                CalculateValue();
            }
        }

        internal void CalculateValue()
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
                valueChanged?.Invoke();
            }
        }
    }
}
