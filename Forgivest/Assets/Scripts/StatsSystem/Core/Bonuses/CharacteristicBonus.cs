using System;
using StatsSystem.Core.Bonuses.Core;
using UnityEngine;

namespace StatsSystem.Core.Bonuses
{
    [Serializable]
    public class CharacteristicBonus : IBonus
    {
        [field: SerializeField] public CharacteristicEnum Characteristic { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        
        public CharacteristicBonus(CharacteristicEnum characteristic, float value)
        {
            Characteristic = characteristic;
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