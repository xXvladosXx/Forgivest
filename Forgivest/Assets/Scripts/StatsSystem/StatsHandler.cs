using System.Collections.Generic;
using Sirenix.OdinInspector;
using StatsSystem.Core;
using StatsSystem.Core.Bonuses;
using StatsSystem.Core.Bonuses.Core;
using StatsSystem.StatContainer;
using UnityEngine;

namespace StatsSystem
{
    public class StatsHandler : SerializedMonoBehaviour
    {
        [field: SerializeField] public StatsContainer StatsContainer { get; private set; }
        [field: SerializeField] public List<IModifier> Modifiers { get; private set; }

        public List<CharacteristicBonus> CharacteristicBonuses { get; private set; } = new List<CharacteristicBonus>();
        public List<StatBonus> StatBonuses { get; private set; } = new List<StatBonus>();

        private void Start()
        {
            CharacteristicBonuses.AddRange(StatsContainer.CharacteristicModifiers);
            StatBonuses.AddRange(StatsContainer.StatModifiers);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                CalculateStat(StatsEnum.Health);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                CalculateStat(StatsEnum.Mana);
            }
        }

        public void CalculateStat(StatsEnum stat)
        {
            var startValue = StatsContainer.CalculateStatFromCharacteristic(stat);

            var bonusList = new List<IBonus>();
            foreach (var modifier in Modifiers)
            {
                bonusList.AddRange(modifier.GetBonus());
            }

            var characteristicValue = CalculateFromModifiers(stat, bonusList);
            Debug.Log(startValue + characteristicValue);
        }

        private float CalculateFromModifiers(StatsEnum stat, List<IBonus> bonusList)
        {
            float startValue = 0;

            foreach (var statDepender in StatsContainer.StatDependers)
            {
                if (statDepender.Key != stat) continue;
                
                foreach (var bonus in bonusList)
                {
                    switch (bonus)
                    {
                        case CharacteristicBonus characteristicBonus:
                            startValue += characteristicBonus.Value * statDepender.Value.ModifierValue;
                            break;
                        case StatBonus statBonus:
                            startValue += statBonus.Value;
                            break;
                    }
                }
            }

            return startValue;
        }
    }
}