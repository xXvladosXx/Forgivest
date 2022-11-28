using AbilitySystem.AbilitySystem.Runtime.Effects.Persistent;
using AttackSystem.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Effects.Stackable
{
    public class GameplayStackableEffect : GameplayPersistentEffect
    {
        public int StackCount;
        public GameplayEffectStackingDuration StackingDuration => Definition.StackingDurationRefresh;
        public new GameplayStackableEffectDefinition Definition => (GameplayStackableEffectDefinition) base.Definition;

        public GameplayStackableEffect(GameplayStackableEffectDefinition definition, object source, GameObject instigator, AttackData attackData) 
            : base(definition, source, instigator, attackData)
        {
            StackCount = 1;
        }
    }
}