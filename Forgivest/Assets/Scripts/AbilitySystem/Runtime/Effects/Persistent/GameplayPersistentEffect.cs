using System;
using AttackSystem.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime
{
    public class GameplayPersistentEffect : GameplayEffect
    {
        public new GameplayPersistentEffectDefinition Definition => (GameplayPersistentEffectDefinition) base.Definition;

        public float RemainingDuration;
        public float RemainingPeriod;
        public float CurrentDuration { get; private set; }
        
        public GameplayPersistentEffect(GameplayPersistentEffectDefinition definition, object source,
            GameObject instigator, AttackData abilityAttackData)
            : base(definition, source, instigator, abilityAttackData)
        {
            RemainingPeriod = definition.Period;
            
            if (!definition.IsInfinite)
            {
                RemainingDuration = CurrentDuration = definition.DurationFormula.CalculateValue(instigator);
            }
        }
    }
}