using System;
using System.Collections.Generic;
using StatsSystem.Core.Bonuses.Core;
using Unity.VisualScripting;

namespace StatsSystem.Core
{
    public interface IModifier
    {
        public event Action<List<IBonus>> OnBonusAdded;
        public event Action<List<IBonus>> OnBonusRemoved;
        
        List<IBonus> GetBonus();
    }
}