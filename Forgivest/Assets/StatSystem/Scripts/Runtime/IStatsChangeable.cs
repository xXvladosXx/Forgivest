using System;
using System.Collections.Generic;
using StatSystem;

namespace InventorySystem
{
    public interface IStatsChangeable
    {
        event Action<List<StatModifier>> OnStatChanged;
    }
}