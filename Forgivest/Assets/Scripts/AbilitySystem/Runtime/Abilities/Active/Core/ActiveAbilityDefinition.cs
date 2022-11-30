using System;
using System.Text;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using AbilitySystem.AbilitySystem.Runtime.Effects.Core;
using AbilitySystem.AbilitySystem.Runtime.Effects.Persistent;
using AttackSystem.Core;
using InventorySystem.Items.Core;
using UnityEngine;

namespace AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core
{
    public abstract class ActiveAbilityDefinition : AbilityDefinition
    {
        [field: SerializeField] public string AnimationName { get; protected set; }
        [field: SerializeField] public GameplayEffectDefinition Cost { get; protected set; }
        [field: SerializeField] public GameplayPersistentEffectDefinition Cooldown { get; private set; }
        [field: SerializeField] public bool SelfCasted { get; private set; }
        
        [field: SerializeField] public Item RequiredWeapon { get; private set; }
        public int HashAnimation => Animator.StringToHash(AnimationName);

        
        public override string ItemDescription
        {
            get
            {
                var stringBuilder = new StringBuilder(base.ItemDescription.ToString());
                if (Cost != null)
                {
                    var cost = new GameplayEffect(Cost, this, User, null);

                    stringBuilder.Append(cost).AppendLine();
                }

                if (Cooldown != null)
                {
                    var cooldown = new GameplayPersistentEffect(Cooldown, this, User, null);
                    stringBuilder.Append(cooldown);
                }
            
                return stringBuilder.ToString();
            }
        }
    }
}