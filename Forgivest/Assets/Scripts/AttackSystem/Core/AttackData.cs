using UnityEngine;

namespace AttackSystem.Core
{
    public class AttackData
    {
        public float Damage { get; set; }
        public IDamageApplier DamageApplier { get; set; }
        public LayerMask DamageApplierLayerMask { get; set; }
    }
}