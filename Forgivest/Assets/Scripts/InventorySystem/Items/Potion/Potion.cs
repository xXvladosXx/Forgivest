using InventorySystem.Items.Core;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using UnityEngine;

namespace InventorySystem.Items.Potion
{
    [CreateAssetMenu (menuName = "InventorySystem/Item/Potion")]
    public class Potion : Item
    {
        [field: SerializeField] public StatModifier StatModifier { get; private set; }
        
        public override bool TryToUseItem(StatController statController)
        {
            statController.Stats.TryGetValue(StatModifier.Name, out var stat);

            if (stat is Attribute attribute)
            {
                attribute.ApplyModifier(StatModifier);
                return true;
            }

            return false;
        }

        public override string ItemDescription { get; }
        public override string ItemRequirements { get; }
    }
}