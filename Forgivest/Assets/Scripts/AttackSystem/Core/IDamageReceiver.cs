using System;
using System.Collections.Generic;
using AttackSystem.Reward.Core;
using UnityEngine;

namespace AttackSystem.Core
{
    public interface IDamageReceiver
    {
        List<IRewardable> Rewards { get; }
        LayerMask LayerMask { get; }
        GameObject GameObject { get; }
        void ReceiveDamage(AttackData attackData);
        event Action<AttackData> OnDamageReceived; 
        
    }
}