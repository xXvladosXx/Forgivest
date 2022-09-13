using UnityEngine;

namespace InventorySystem.Items.Weapon
{
    public abstract class Weapon : StatsableItem
    {
        [field: SerializeField] public bool RightHanded { get; private set; }
    }
}