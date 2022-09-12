using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem.Core.StatContainer
{
    public class StatsHandler : MonoBehaviour
    {
        [field: SerializeField] public StatsContainer StatsContainer { get; private set; }

        private List<IModifier> _modifiers = new List<IModifier>();

        public void Init(List<IModifier> modifiers)
        {
            _modifiers = modifiers;
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
            Debug.Log(startValue);
        }
    }
}