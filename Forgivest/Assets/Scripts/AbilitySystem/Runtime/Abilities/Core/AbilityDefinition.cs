using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities
{
    public abstract class AbilityDefinition : ScriptableObject
    {
        [SerializeField] private List<GameplayEffectDefinition> _gameplayEffectDefinitions;
        public ReadOnlyCollection<GameplayEffectDefinition> GameplayEffectDefinitions => _gameplayEffectDefinitions.AsReadOnly();

        [SerializeField] private int _maxLevel;
        public int MaxLevel => _maxLevel;
        
        [SerializeField] private int _level = 5;
        public int Level => _level;
    }
}