using System;
using UnityEngine;

namespace StatSystem
{
    public enum ModifierOperationType
    {
        Additive,
        Multiplicative,
        Override
    }
    
    [Serializable]
    public class StatModifier
    {
        [field: SerializeField] public object Source { get; set; }
        [field: SerializeField] public float Magnitude { get; set; }
        [field: SerializeField] public ModifierOperationType Type { get; set; }
    }
}