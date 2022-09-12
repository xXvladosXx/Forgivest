using System.Collections.Generic;
using Sirenix.OdinInspector;
using StatsSystem.Core;
using StatsSystem.Core.Bonuses;
using UnityEngine;

namespace StatsSystem.StatContainer
{
    [CreateAssetMenu (menuName = "Stats/StatsContainer")]
    public class StatsContainer : SerializedScriptableObject
    {
        [field: SerializeField] public List<StatBonus> StatModifiers { get; private set; }
        [field: SerializeField] public List<CharacteristicBonus> CharacteristicModifiers { get; private set; }
        [field: SerializeField] public Dictionary<StatsEnum, StatDepender> StatDependers { get; private set; }
        public float CalculateStatFromCharacteristic(StatsEnum stats)
        {
            float finalValue = 0;
            
            foreach (var statDepender in StatDependers)
            {
                if (statDepender.Key == stats)
                {
                    foreach (var characteristicModifier in CharacteristicModifiers)
                    {
                        if (characteristicModifier.Characteristic == statDepender.Value.Characteristic)
                        {
                            finalValue = characteristicModifier.Value * statDepender.Value.ModifierValue;
                        }
                    }
                }
            }

            return finalValue;
        }
    }
}