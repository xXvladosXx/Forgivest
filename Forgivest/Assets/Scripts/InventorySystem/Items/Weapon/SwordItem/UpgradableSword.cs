using InventorySystem.Items.Core;
using InventorySystem.Items.Weapon.SwordItem;
using UnityEngine;

namespace InventorySystem.Items.Weapon
{
    [CreateAssetMenu (menuName = "InventorySystem/Item/UpgradableSword")]
    public class UpgradableSword : Sword, IUpgradable
    {
        [field: SerializeField] public Item NextLevel { get; private set; }
    }
}