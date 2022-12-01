using Core;
using StatsSystem.Scripts.Runtime;
using UnityEngine;

namespace StatSystem
{
    public class HealthModifier : StatModifier, IDamage
    {
        public bool IsCriticalHit { get; set; }
        public GameObject Instigator { get; set; }
    }
}