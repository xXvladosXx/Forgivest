using Core;
using UnityEngine;

namespace StatsSystem.Scripts.Runtime
{
    public class HealthModifier : StatModifier, IDamage
    {
        public bool IsCriticalHit { get; set; }
        public GameObject Instigator { get; set; }
    }
}