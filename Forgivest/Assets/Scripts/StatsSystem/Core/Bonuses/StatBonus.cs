using System;
using StatsSystem.Core.Bonuses.Core;
using UnityEngine;

namespace StatsSystem.Core.Bonuses
{
    [Serializable]
    public class StatBonus : IBonus
    {
        [field: SerializeField] public StatsEnum Stat { get; private set; }
        [field: SerializeField] public float Value { get; private set; }

        public StatBonus(StatsEnum stat, float value)
        {
            Stat = stat;
            Value = value;
        }
    }
    
    
}