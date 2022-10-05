using System;
using System.Collections.Generic;
using AbilitySystem.AbilitySystem.Runtime;
using SaveSystem.Scripts.Runtime;
using StatSystem.Nodes;
using UnityEngine;

namespace StatSystem
{
    [RequireComponent(typeof(TagController))]
    public class StatController : MonoBehaviour
    {
        [SerializeField] private StatDatabase m_StatDatabase;
        protected Dictionary<string, Stat> m_Stats = new Dictionary<string, Stat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Stat> stats => m_Stats;
        private TagController _tagController;
        public bool IsInitialized { get; private set; }
        public event Action OnInitialized;
        public event Action willUninitialize;

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
            willUninitialize?.Invoke();
        }

        protected void Initialize()
        {
            foreach (StatDefinition definition in m_StatDatabase.stats)
            {
                m_Stats.Add(definition.name, new Stat(definition, this));
            }

            foreach (StatDefinition definition in m_StatDatabase.attributes)
            {
                if (definition.name.Equals("Health", StringComparison.OrdinalIgnoreCase))
                {
                    m_Stats.Add(definition.name, new Health(definition, this, _tagController));
                }
                else
                {
                    m_Stats.Add(definition.name, new Attribute(definition, this));   
                }
            }

            foreach (StatDefinition definition in m_StatDatabase.primaryStats)
            {
                m_Stats.Add(definition.name, new PrimaryStat(definition, this));
            }
            
            InitializeStatFormulas();

            foreach (Stat stat in m_Stats.Values)
            {
                stat.Initialize();
            }
            
            IsInitialized = true;
            OnInitialized?.Invoke();
        }

        private void Update()
        {
            foreach (var stat in m_Stats.Values)
            {
                if (stat.definition.name == "Health")
                {
                    print(stat.value);
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
            }
        }
        
        protected virtual void InitializeStatFormulas()
        {
            foreach (Stat currentStat in m_Stats.Values)
            {
                if (currentStat.definition.Formula != null && currentStat.definition.Formula.rootNode != null)
                {
                    List<StatNode> statNodes = currentStat.definition.Formula.FindNodesOfType<StatNode>();

                    foreach (StatNode statNode in statNodes)
                    {
                        if (m_Stats.TryGetValue(statNode.statName.Trim(), out Stat stat))
                        {
                            statNode.stat = stat;
                            stat.valueChanged += currentStat.CalculateValue;
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