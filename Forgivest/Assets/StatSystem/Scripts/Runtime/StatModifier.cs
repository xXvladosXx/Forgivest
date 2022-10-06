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
        [field: SerializeField] public string StatName { get; private set; }
        [field: SerializeField] public object Source { get; set; }
        [field: SerializeField] public float Magnitude { get; set; }
        [field: SerializeField] public ModifierOperationType Type { get; set; }
    }
}