using System;
using AttackSystem.Core;
using AttackSystem.Reward.Core;
using UnityEngine;

namespace AttackSystem.Reward
{
    [Serializable]
    public class ExperienceReward : IRewardable
    {
        [SerializeField] private int _amount;
        public int Amount => _amount;
    }
}