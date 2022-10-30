using System;
using AttackSystem.Reward.Core;
using InventorySystem.Items.Core;
using UnityEngine;

namespace AttackSystem.Reward
{
    [Serializable]
    public class ItemReward : IRewardable
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _amount;
        
        public Item Item => _item;
        public int Amount => _amount;
    }
}