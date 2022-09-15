using System;
using ModestTree.Util;
using UnityEngine;

namespace AttackSystem.Core
{
    public interface IDamageApplier
    {
        event Action<AttackData> OnDamageApplied; 

        void ApplyDamage(AttackData attackData, float timeOfActivation);
    }
}