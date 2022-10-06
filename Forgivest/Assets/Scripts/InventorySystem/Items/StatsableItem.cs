using System.Collections.Generic;
using AbilitySystem.AbilitySystem.Runtime;
using InventorySystem.Items.Core;
using StatSystem;
//using StatsSystem.Core.Bonuses;
using UnityEngine;

namespace InventorySystem.Items
{
    public abstract class StatsableItem : Item, IStatsable
    {
        //[field: SerializeField] public List<CharacteristicBonus> CharacteristicBonuses { get; private set; }
        //[field: SerializeField] public List<StatBonus> StatsBonuses { get; private set; }
        
        [field: SerializeField] public List<StatModifier> StatModifier { get; private set; }
    }
}