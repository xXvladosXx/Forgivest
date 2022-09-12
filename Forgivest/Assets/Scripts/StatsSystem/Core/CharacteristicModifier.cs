using System;
using StatsSystem.Core.StatType.Core;
using UnityEngine;

namespace StatsSystem.Core
{
    [Serializable]
    public class CharacteristicModifier
    {
        [field: SerializeField] public CharacteristicEnum Characteristic { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        
        public CharacteristicModifier(CharacteristicEnum characteristic, float value)
        {
            Characteristic = characteristic;
            Value = value;
        }
    }
}