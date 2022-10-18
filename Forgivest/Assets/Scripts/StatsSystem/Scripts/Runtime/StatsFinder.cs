using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StatSystem.Scripts.Runtime
{
    public class StatsFinder : SerializedMonoBehaviour
    {
        private StatController _statController;
        private List<IStatsChangeable> _statsChangeables = new List<IStatsChangeable>();
        private List<StatModifier> _statModifiers = new List<StatModifier>();
        private void Awake()
        {
            _statController = GetComponent<StatController>();
            _statsChangeables = GetComponents<IStatsChangeable>().ToList();
        }

        private void OnEnable()
        {
            foreach (var statsChangeable in _statsChangeables)
            {
                statsChangeable.OnStatAdded += OnStatAdded;
                statsChangeable.OnStatRemoved += OnStatRemoved;
            }
        }

       
        private void OnDisable()
        {
            foreach (var statsChangeable in _statsChangeables)
            {
                statsChangeable.OnStatAdded -= OnStatAdded;
                statsChangeable.OnStatRemoved -= OnStatRemoved;
            }
        }

        private void OnStatAdded(List<StatModifier> modifiers)
        {
            foreach (var statModifier in modifiers)
            {
                _statController.AddStat(statModifier);
            }
        }
        
        private void OnStatRemoved(List<StatModifier> modifiers)
        {
            foreach (var statModifier in modifiers)
            {
                _statController.RemoveStat(statModifier);
            }
        }

        public float FindStat(string stat)
        {
            _statController.Stats.TryGetValue(stat, out var statValue);

            if (statValue != null) return statValue.value;

            return 0;
        }
    }
}