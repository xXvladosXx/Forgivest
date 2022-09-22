using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime
{
    public class GameplayPersistentEffect : GameplayEffect
    {
        public new GameplayPersistentEffectDefinition Definition => (GameplayPersistentEffectDefinition) base.Definition;

        public float RemainingDuration;
        public float CurrentDuration { get; private set; }
        
        public GameplayPersistentEffect(GameplayPersistentEffectDefinition definition, object source, GameObject instigator)
            : base(definition, source, instigator)
        {
            if (!definition.IsInfinite)
            {
                RemainingDuration = CurrentDuration = definition.DurationFormula.CalculateValue(instigator);
            }
        }
    }
}