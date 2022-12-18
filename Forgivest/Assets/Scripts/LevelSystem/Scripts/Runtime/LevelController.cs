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
        private bool _isInitialized;
        
        private const int _experienceToNextLevel = 100;
        public int Level => _level;
        public event Action OnLevelChanged;
        public event Action<int, int, int> OnCurrentExperienceChanged;

        public int CurrentExperience
        {
            get => _currentExperience;
            set
            {
                if (value >= RequiredExperience)
                {
                    _currentExperience = value - RequiredExperience;
                    _level++;
                    OnCurrentExperienceChanged?.Invoke(_currentExperience, RequiredExperience, _level);
                    OnLevelChanged?.Invoke();
                }
                else if (value < RequiredExperience)
                {
                    _currentExperience = value;
                    OnCurrentExperienceChanged?.Invoke(_currentExperience, RequiredExperience, _level);
                }
            }
        }

        public int RequiredExperience => Mathf.RoundToInt(_experienceToNextLevel * _level);
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