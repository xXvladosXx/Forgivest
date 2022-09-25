using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbilitySystem.AbilitySystem.Runtime.VisualEffects;
using Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime
{
    [EffectType(typeof(GameplayPersistentEffect))]
    [CreateAssetMenu(fileName = "GameplayPersistentEffect", menuName = "AbilitySystem/Effect/GameplayPersistentEffect", order = 0)]
    public class GameplayPersistentEffectDefinition : GameplayEffectDefinition
    {
        [SerializeField] private bool _isInfinite;
        public bool IsInfinite
        {
            get => _isInfinite;
            protected set => _isInfinite = value;
        }

        [SerializeField] private NodeGraph _durationFormula;
        public NodeGraph DurationFormula
        {
            get => _durationFormula;
            protected set => _durationFormula = value;
        }

        [SerializeField] private bool _isPeriodic;
        public bool IsPeriodic
        {
            get => _isPeriodic;
            protected set => _isPeriodic = value;
        }
        
        [SerializeField] private float _period;
        public float Period
        {
            get => _period;
            protected set => _period = value;
        }
        
        [SerializeField] private bool _isExecutableEffectOnApply;
        public bool IsExecutableEffectOnApply
        {
            get => _isExecutableEffectOnApply;
            protected set => _isExecutableEffectOnApply = value;
        }

        [SerializeField] private List<string> _grantedTags;
        public ReadOnlyCollection<string> GrantedTags => _grantedTags.AsReadOnly();

        [SerializeField] private SpecialEffectDefinition _specialPersistentEffectDefinition;

        public SpecialEffectDefinition SpecialPersistentEffectDefinition => _specialPersistentEffectDefinition;
        
    }
}