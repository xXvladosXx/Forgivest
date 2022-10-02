using UnityEngine;

namespace StatSystem
{
    public enum ModifierOperationType
    {
        Additive,
        Multiplicative,
        Override
    }
    
    public class StatModifier
    {
        public object Source { get; set; }
        public int Magnitude { get; set; }
        public ModifierOperationType Type { get; set; }
    }
}