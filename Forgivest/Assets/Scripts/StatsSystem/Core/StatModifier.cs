using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem.Core
{
    [Serializable]
    public class StatModifier
    {
        [field: SerializeField] public StatsEnum Stat { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public StatDepender StatDepender { get; private set; }

        public StatModifier(StatsEnum stat, StatDepender statDepender, float value)
        {
            Stat = stat;
            StatDepender = statDepender;
            Value = value;
        }
    }
    
    [Serializable]
    public class StatDepender
    {
        [field: SerializeField] public CharacteristicEnum Characteristic { get; private set; }
        [field: SerializeField] public float ModifierValue { get; private set; }
    }
}