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
        private void Awake()
        {
            _statController = GetComponent<StatController>();
            _statsChangeables = GetComponents<IStatsChangeable>().ToList();
        }

        private void OnEnable()
        {
            foreach (var statsChangeable in _statsChangeables)
            {
                statsChangeable.OnStatChanged += OnStatChanged;
            }
        }

        private void OnDisable()
        {
            foreach (var statsChangeable in _statsChangeables)
            {
                statsChangeable.OnStatChanged -= OnStatChanged;
            }
        }

        private void OnStatChanged(List<StatModifier> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                _statController.AddStat(modifier);
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