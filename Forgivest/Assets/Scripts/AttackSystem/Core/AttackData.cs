using InventorySystem.Items.Weapon;
using UnityEngine;

namespace AttackSystem.Core
{
    public class AttackData
    {
        public float Damage { get; set; }
        public IDamageApplier DamageApplier { get; set; }
        public IDamageReceiver DamageReceiver { get; set; }
        public Weapon Weapon { get; set; }
        public Object Source { get; set; }

        public LayerMask DamageApplierLayerMask { get; set; }
        public bool IsCritical { get; set; } = false;
    }
}