using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StatsSystem.Core.StatContainer
{
    [CreateAssetMenu (menuName = "Stats/StatsContainer")]
    public class StatsContainer : SerializedScriptableObject
    {
        [field: SerializeField] public List<StatModifier> StatModifiers { get; private set; }
        [field: SerializeField] public List<CharacteristicModifier> CharacteristicModifiers { get; private set; }

        public float CalculateStatFromCharacteristic(StatsEnum stats)
        {
            float finalValue = 0;
            
            foreach (var characteristicModifier in CharacteristicModifiers)
            {
                foreach (var statModifier in StatModifiers)
                {
                    if(statModifier.Stat != stats) continue;
                    
                    if (characteristicModifier.Characteristic == statModifier.StatDepender.Characteristic)
                    {
                        finalValue += characteristicModifier.Value * statModifier.StatDepender.ModifierValue;
                    }
                }
            }

            return finalValue;
        }
    }
}