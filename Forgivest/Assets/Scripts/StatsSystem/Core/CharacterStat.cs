using System;
using System.Collections.Generic;
using StatsSystem.Core.StatType;

namespace StatsSystem.Core
{
    public class CharacterStat
    {
        private readonly List<CharacteristicModifier> _statModifiers = new List<CharacteristicModifier>();

        public CharacterStat(IEnumerable<CharacteristicModifier> statModifiers)
        {
            _statModifiers.AddRange(statModifiers);
        }
        
        public void AddStatModifier(CharacteristicModifier characteristicModifier)
        {
            _statModifiers.Add(characteristicModifier);
        }

        public void RemoveStatModifier(CharacteristicModifier characteristicModifier)
        {
            _statModifiers.Remove(characteristicModifier);
        }

        public float CalculateFinalValue()
        {
            float finalValue = 0;


            return (float) Math.Round(finalValue, 4);
        }
    }
}