using System.Collections.Generic;
using System.Text;
using AbilitySystem.AbilitySystem.Runtime;
using InventorySystem.Items.Core;
using StatsSystem.Scripts.Runtime;
using StatSystem;
//using StatsSystem.Core.Bonuses;
using UnityEngine;

namespace InventorySystem.Items
{
    public abstract class StatsableItem : Item, IStatsable
    {
        [field: SerializeField] public List<StatModifier> StatModifier { get; private set; }

        public override string ItemDescription 
        {
            get
            {
                var stringBuilder = new StringBuilder();
                
                if (Rarity != null)
                {
                    stringBuilder.Append(Rarity.Name).AppendLine().AppendLine();
                }
                
                foreach (var statModifier in StatModifier)
                {
                    stringBuilder.Append(statModifier.Name).Append(": ").Append(statModifier.Magnitude).AppendLine();
                }

                return stringBuilder.ToString();
            }

            protected set => ItemDescription = value;
        }
    }
}