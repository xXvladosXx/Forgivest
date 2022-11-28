using System;
using System.Collections.Generic;

namespace StatsSystem.Scripts.Runtime
{
    public interface IStatsChangeable
    {
        event Action<List<StatModifier>> OnStatAdded;
        event Action<List<StatModifier>> OnStatRemoved;
    }
}