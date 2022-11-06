using System.Collections.Generic;
using System.Collections.ObjectModel;
using InventorySystem.Items.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities
{
    public abstract class AbilityDefinition : Item
    {
        [field: SerializeField] public override string ItemDescription { get; protected set; }

        [SerializeField] private List<GameplayEffectDefinition> _gameplayEffectDefinitions;
        public ReadOnlyCollection<GameplayEffectDefinition> GameplayEffectDefinitions => _gameplayEffectDefinitions.AsReadOnly();

        [SerializeField] private int _maxLevel;
        public int MaxLevel => _maxLevel;
        
        [SerializeField] private int _level = 5;
        public int Level => _level;

        [SerializeField] private int _requiredLevel;
        [SerializeField] private int _requiredAbilityPoints;
        
        public bool RequirementsChecked(int level, int abilityPoints)
        {
            return level >= _requiredLevel && abilityPoints >= _requiredAbilityPoints;
        }
    }
}