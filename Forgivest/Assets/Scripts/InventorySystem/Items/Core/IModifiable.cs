using System.Collections.Generic;
using StatsSystem.Scripts.Runtime;

//using StatsSystem.Core.Bonuses;

namespace InventorySystem.Items.Core
{
    public interface IStatsable
    {
        //List<CharacteristicBonus> CharacteristicBonuses { get; }
        //List<StatBonus> StatsBonuses { get; }
        
         public List<StatModifier> StatModifier { get;  }
    }
}