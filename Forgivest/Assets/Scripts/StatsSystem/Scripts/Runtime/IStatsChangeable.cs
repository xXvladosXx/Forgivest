using System;
using System.Collections.Generic;
using StatsSystem.Scripts.Runtime;
using StatSystem;

namespace InventorySystem
{
    public interface IStatsChangeable
    {
        event Action<List<StatModifier>> OnStatAdded;
        event Action<List<StatModifier>> OnStatRemoved;
    }
}