using System;
using System.Collections.Generic;
using Core;
using LevelSystem.Nodes;
using SaveSystem.Scripts.Runtime;
using UnityEngine;

namespace LevelSystem
{
    public class LevelController : MonoBehaviour, ILevelable
    {
        [SerializeField] private int m_Level = 1;
        [SerializeField] private int m_CurrentExperience;
        [SerializeField] private NodeGraph m_RequiredExperienceFormula;

        private bool m_IsInitialized;

        public int Level => m_Level;
        public event Action OnLevelChanged;
        public event Action OnCurrentExperienceChanged;

        public int CurrentExperience
        {
            get => m_CurrentExperience;
            set
            {
                if (value >= RequiredExperience)
                {
                    m_CurrentExperience = value - RequiredExperience;
                    OnCurrentExperienceChanged?.Invoke();
                    m_Level++;
                    OnLevelChanged?.Invoke();
                }
                else if (value < RequiredExperience)
                {
                    m_CurrentExperience = value;
                    OnCurrentExperienceChanged?.Invoke();
                }
            }
        }

        public int RequiredExperience => Mathf.RoundToInt(m_RequiredExperienceFormula.rootNode.Value);
        public bool IsInitialized => m_IsInitialized;
        public event Action OnInitialized;
        public event Action OnWillUninitialize;

        private void Awake()
        {
            if (!m_IsInitialized)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            List<LevelNode> levelNodes = m_RequiredExperienceFormula.FindNodesOfType<LevelNode>();
            foreach (LevelNode levelNode in levelNodes)
            {
                levelNode.Levelable = this;
            }

            m_IsInitialized = true;
            OnInitialized?.Invoke();
        }

      
        private void OnDestroy()
        {
            OnWillUninitialize?.Invoke();
        }
    }
}