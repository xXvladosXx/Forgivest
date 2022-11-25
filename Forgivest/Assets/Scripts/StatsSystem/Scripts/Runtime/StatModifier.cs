using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace StatsSystem.Scripts.Runtime
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
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string StatName { get; private set; }
        [field: SerializeField] public object Source { get; set; }
        [field: SerializeField] public float Magnitude { get; set; }

        [field: SerializeField]
        public string MagnitudeForDescription
        {
            get
            {
                var str = $"{Magnitude}";
                return Regex.Replace(str, @"-", "");
            }
            set
            {
                
            }
        }

        [field: SerializeField] public ModifierOperationType Type { get; set; }

        public override string ToString()
        {
            return Magnitude.ToString();
        }
    }
}