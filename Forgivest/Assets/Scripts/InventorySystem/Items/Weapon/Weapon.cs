using UnityEditor.Animations;
using UnityEngine;

namespace InventorySystem.Items.Weapon
{
    public abstract class Weapon : StatsableItem
    {
        [field: SerializeField] public bool RightHanded { get; private set; }
        [field: SerializeField] public float AttackRate { get; private set; }

        [field: SerializeField] public AnimatorController AnimatorController { get; private set; }
    }
}