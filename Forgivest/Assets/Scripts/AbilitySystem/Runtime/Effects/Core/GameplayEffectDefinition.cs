using System.Collections.Generic;
using System.Collections.ObjectModel;
using AttackSystem.VisualEffects;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Effects.Core
{
    [EffectType(typeof(GameplayEffect))]
    [CreateAssetMenu(fileName = "GameplayEffect", menuName = "AbilitySystem/Effect/GameplayEffect", order = 0)]
    public class GameplayEffectDefinition : ScriptableObject
    {
        [SerializeField] private string _description;
        public string Description => _description;
        
        [SerializeField] private List<BaseGameplayEffectStatModifier> _modifiers;
        public ReadOnlyCollection<BaseGameplayEffectStatModifier> Modifiers => _modifiers.AsReadOnly();
        
        [SerializeField] private List<GameplayEffectDefinition> _conditionalEffects;
        public ReadOnlyCollection<GameplayEffectDefinition> ConditionalEffects => _conditionalEffects.AsReadOnly();

        [SerializeField] private SpecialEffectDefinition _specialEffectDefinition;
        public SpecialEffectDefinition SpecialEffectDefinition => _specialEffectDefinition;

        [SerializeField] private List<string> _tags;
        public ReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        [SerializeField] private List<string> _removeEffectsWithTags;
        public ReadOnlyCollection<string> RemoveEffectsWithTags => _removeEffectsWithTags.AsReadOnly();

        [SerializeField] private List<string> _applicationMustBePresentTags;
        public ReadOnlyCollection<string> ApplicationMustBePresentTags => _applicationMustBePresentTags.AsReadOnly();
        [SerializeField] private List<string> _applicationMustBeAbsentTags;
        public ReadOnlyCollection<string> ApplicationMustBeAbsentTags => _applicationMustBeAbsentTags.AsReadOnly();
    }
}