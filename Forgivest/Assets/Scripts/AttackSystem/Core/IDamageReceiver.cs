using System;
using UnityEngine;

namespace AttackSystem.Core
{
    public interface IDamageReceiver
    {
        event Action<AttackData> OnDamageReceived; 
        
        LayerMask LayerMask { get; }
        void ReceiverDamage(AttackData attackData);
    }
}