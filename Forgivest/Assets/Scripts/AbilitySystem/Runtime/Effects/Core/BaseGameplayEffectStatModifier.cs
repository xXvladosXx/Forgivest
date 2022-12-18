using Core;
using StatsSystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Effects.Core
{
    public abstract class BaseGameplayEffectStatModifier : ScriptableObject
    {
#if UNITY_EDITOR

        [field: SerializeField] public NodeGraph Formula { get; private set; }
#endif        
        public abstract string StatName { get; protected set; }
        public abstract ModifierOperationType Type { get; protected set; }
    }
}