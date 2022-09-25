using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Effects.Stackable
{
    public enum GameplayEffectStackingExpiration
    {
        NeverRefresh,
        RemoveSingleStackAndRefreshDuration
    }

    public enum GameplayEffectStackingDuration
    {
        NeverRefresh,
        RefreshOnSuccessfulApplication
    }

    public enum GameplayEffectStackingPeriod
    {
        NeverReset,
        ResetOnSuccessfulApplication
    }
    
    [EffectType(typeof(GameplayStackableEffect))]
    [CreateAssetMenu(fileName = "GameplayStackableEffect", menuName = "AbilitySystem/Effect/GameplayStackableEffect", order = 0)]
    public class GameplayStackableEffectDefinition : GameplayPersistentEffectDefinition
    {
        [SerializeField] private List<GameplayEffectDefinition> _overflowEffects;
        public ReadOnlyCollection<GameplayEffectDefinition> OverflowEffects => _overflowEffects.AsReadOnly();

        [SerializeField] private bool _denyOverflowApplication;
        public bool DenyOverflowApplication => _denyOverflowApplication;

        [SerializeField] private bool _clearStackOnOverflow;
        public bool ClearStackOnOverflow => _clearStackOnOverflow;

        [SerializeField] private int _stackCountLimit = 3;
        public int StackCountLimit => _stackCountLimit;

        [SerializeField] private GameplayEffectStackingDuration _stackingDuration;
        public GameplayEffectStackingDuration StackingDurationRefresh => _stackingDuration;

        [SerializeField] private GameplayEffectStackingPeriod _stackingPeriod;
        public GameplayEffectStackingPeriod StackingPeriodReset => _stackingPeriod;
        
        [SerializeField] private GameplayEffectStackingExpiration _stackingExpiration;
        public GameplayEffectStackingExpiration StackingExpiration => _stackingExpiration;
    }
}