using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Effects.Stackable
{
    public class GameplayStackableEffect : GameplayPersistentEffect
    {
        public int StackCount;
        public new GameplayStackableEffectDefinition Definition => (GameplayStackableEffectDefinition) base.Definition;

        public GameplayStackableEffect(GameplayStackableEffectDefinition definition, object source, GameObject instigator) 
            : base(definition, source, instigator)
        {
            StackCount = 1;
        }
    }
}