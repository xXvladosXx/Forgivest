using System;
using System.Collections.Generic;
using AbilitySystem.AbilitySystem.Runtime;
using SaveSystem.Scripts.Runtime;
using StatsSystem.Scripts.Runtime;
using StatSystem.Nodes;
using UnityEngine;

namespace StatSystem
{
    [RequireComponent(typeof(TagController))]
    public class StatController : MonoBehaviour
    {
        [SerializeField] private StatDatabase _statDatabase;
        protected Dictionary<string, Stat> _stats = new Dictionary<string, Stat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Stat> Stats => _stats;
        private TagController _tagController;
        public Health Health { get; private set; }
        public bool IsInitialized { get; private set; }
        public event Action OnInitialized;
        public event Action WillUninitialize;

        public event Action<Attribute> OnStatChanged;

        protected virtual void Awake()
        {
            _tagController = GetComponent<TagController>();
            
            if (!IsInitialized)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            WillUninitialize?.Invoke();
        }

        protected void Initialize()
        {
            foreach (StatDefinition definition in _statDatabase.stats)
            {
                _stats.Add(definition.name, new Stat(definition, this));
            }

            foreach (StatDefinition definition in _statDatabase.attributes)
            {
                if (definition.name.Equals("Health", StringComparison.OrdinalIgnoreCase))
                {
                    Health = new Health(definition, this, _tagController);
                    _stats.Add(definition.name, Health);
                }
                else
                {
                    _stats.Add(definition.name, new Attribute(definition, this));   
                }
            }

            foreach (StatDefinition definition in _statDatabase.primaryStats)
            {
                _stats.Add(definition.name, new PrimaryStat(definition, this));
            }
            
            InitializeStatFormulas();

            foreach (Stat stat in _stats.Values)
            {
                stat.Initialize();
            }
            
            IsInitialized = true;
            OnInitialized?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                _stats["Health"].AddModifier(new StatModifier
                {
                    Magnitude = 10,
                    Type = ModifierOperationType.Additive,
                    Source = this
                });
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                _stats["Strength"].AddModifier(new StatModifier
                {
                    Magnitude = 10,
                    Type = ModifierOperationType.Additive,
                    Source = this
                });
            }
        }

        public void AddStat(StatModifier statModifier)
        {
            _stats[statModifier.StatName].AddModifier(statModifier);
        }

        public void RemoveStat(StatModifier statModifier)
        {
            _stats[statModifier.StatName].RemoveModifier(statModifier);
        }
        
        protected virtual void InitializeStatFormulas()
        {
            foreach (Stat currentStat in _stats.Values)
            {
                if (currentStat.definition.Formula != null && currentStat.definition.Formula.rootNode != null)
                {
                    List<StatNode> statNodes = currentStat.definition.Formula.FindNodesOfType<StatNode>();

                    foreach (StatNode statNode in statNodes)
                    {
                        if (_stats.TryGetValue(statNode.statName.Trim(), out Stat stat))
                        {
                            statNode.stat = stat;
                            stat.OnValueChanged += currentStat.CalculateOnValue;
                        }
                        else
                        {
                            Debug.LogWarning($"Stat {statNode.statName.Trim()} does not exist!");
                        }
                    }
                }
            }
        }
    }
}