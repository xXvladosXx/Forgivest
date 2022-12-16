using System;
using System.Collections.Generic;
using Core;
using LevelSystem.Scripts.Runtime.Nodes;
using UnityEngine;

namespace LevelSystem.Scripts.Runtime
{
    public class LevelController : MonoBehaviour, ILevelable
    {
        [SerializeField] private int _level = 1;
        [SerializeField] private int _currentExperience;
        [SerializeField] private int _requiredExperienceFormula;
        private bool _isInitialized;
        public int Level => _level;
        public event Action OnLevelChanged;
        public event Action<int, int> OnCurrentExperienceChanged;

        public int CurrentExperience
        {
            get => _currentExperience;
            set
            {
                if (value >= RequiredExperience)
                {
                    _currentExperience = value - RequiredExperience;
                    OnCurrentExperienceChanged?.Invoke(_currentExperience, RequiredExperience);
                    _level++;
                    OnLevelChanged?.Invoke();
                }
                else if (value < RequiredExperience)
                {
                    _currentExperience = value;
                    OnCurrentExperienceChanged?.Invoke(_currentExperience, RequiredExperience);
                }
            }
        }

        public int RequiredExperience => Mathf.RoundToInt(_requiredExperienceFormula);
        public bool IsInitialized => _isInitialized;
        public event Action OnInitialized;
        public event Action OnWillUninitialize;

        private void Awake()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            _isInitialized = true;
            OnInitialized?.Invoke();
        }

      
        private void OnDestroy()
        {
            OnWillUninitialize?.Invoke();
        }
    }
}