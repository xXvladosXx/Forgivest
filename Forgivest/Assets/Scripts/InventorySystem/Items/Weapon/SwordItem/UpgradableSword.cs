﻿using InventorySystem.Items.Core;
using UnityEngine;

namespace InventorySystem.Items.Weapon.SwordItem
{
    [CreateAssetMenu (menuName = "InventorySystem/Item/UpgradableSword")]
    public class UpgradableSword : Sword, IUpgradable
    {
        [field: SerializeField] public Item NextLevel { get; private set; }
    }
}