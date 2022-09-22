using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core
{
    public abstract class ActiveAbilityDefinition : AbilityDefinition
    {
        [field: SerializeField] public string AnimationName { get; protected set; }
        [field: SerializeField] public GameplayEffectDefinition Cost { get; protected set; }
        [field: SerializeField] public GameplayPersistentEffectDefinition Cooldown { get; private set; }
    }
}