using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbilitySystem.AbilitySystem.Runtime.VisualEffects;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime
{
    [EffectType(typeof(GameplayEffect))]
    [CreateAssetMenu(fileName = "GameplayEffect", menuName = "AbilitySystem/Effect/GameplayEffect", order = 0)]
    public class GameplayEffectDefinition : ScriptableObject
    {
        [SerializeField] private List<BaseGameplayEffectStatModifier> _modifiers;
        public ReadOnlyCollection<BaseGameplayEffectStatModifier> Modifiers => _modifiers.AsReadOnly();
        
        [field: SerializeField] public SpecialEffectDefinition SpecialEffectDefinition { get; private set; }

    }
}