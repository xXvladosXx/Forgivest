using System;
using InventorySystem.Items;
using InventorySystem.Items.Core;
using InventorySystem.Items.ItemTypes;
using InventorySystem.Items.ItemTypes.Core;
using UnityEngine;

namespace InventorySystem.Core
{
    public interface IItem
    {
        string Name { get; }
        GameObject Prefab { get; }
        Type Type { get; }
        int MaxItemsInStack { get; }
        ItemID ItemID { get; }
    }
}