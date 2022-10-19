using System;
using System.Collections.Generic;
using LevelSystem;
using LevelSystem.Nodes;
using StatsSystem.Scripts.Runtime;
using UnityEngine;

namespace StatSystem
{
    [RequireComponent(typeof(ILevelable))]
    public class PlayerStatController : StatController
    {
        protected ILevelable m_Levelable;

        public event Action OnStatPointsChanged;

        protected override void Awake()
        {
            m_Levelable = GetComponent<ILevelable>();
        }

        private void OnEnable()
        {
            m_Levelable.OnInitialized += OnLevelableOnInitialized;
            m_Levelable.OnWillUninitialize += UnregisterEvents;
            if (m_Levelable.IsInitialized)
            {
                OnLevelableOnInitialized();
            }
        }

        private void OnDisable()
        {
            m_Levelable.OnInitialized -= OnLevelableOnInitialized;
            m_Levelable.OnWillUninitialize -= UnregisterEvents;
            if (m_Levelable.IsInitialized)
            {
                UnregisterEvents();
            }
        }

        private void OnLevelableOnInitialized()
        {
            Initialize();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            m_Levelable.OnLevelChanged += OnLevelChanged;
        }
        
        private void UnregisterEvents()
        {
            m_Levelable.OnLevelChanged -= OnLevelChanged;
        }

        private void OnLevelChanged()
        {
        }

        protected override void InitializeStatFormulas()
        {
            base.InitializeStatFormulas();
            foreach (Stat currentStat in _stats.Values)
            {
                if (currentStat.Definition.Formula != null && currentStat.Definition.Formula.rootNode != null)
                {
                    List<LevelNode> levelNodes = currentStat.Definition.Formula.FindNodesOfType<LevelNode>();
                    foreach (LevelNode levelNode in levelNodes)
                    {
                        levelNode.Levelable = m_Levelable;
                        m_Levelable.OnLevelChanged += () => currentStat.CalculateOnValue(0);
                    }
                }
            }
        }
    }
}