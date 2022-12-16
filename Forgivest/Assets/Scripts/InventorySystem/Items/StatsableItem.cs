using System.Collections.Generic;
using System.Text;
using InventorySystem.Items.Core;
using StatsSystem.Scripts.Runtime;
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
        }

        public override string ItemRequirements
        {
            get
            {
                var stringBuilder = new StringBuilder();
                if (Requirements == null) return stringBuilder.ToString();
                
                foreach (var itemRequirement in Requirements)
                {
                    if (itemRequirement is LevelRequirement levelRequirement)
                    {
                        stringBuilder.Append("Required Level: ").Append(levelRequirement.NecessaryLevel).AppendLine();
                    }
                }

                return stringBuilder.ToString();
            }
        }
    }
}