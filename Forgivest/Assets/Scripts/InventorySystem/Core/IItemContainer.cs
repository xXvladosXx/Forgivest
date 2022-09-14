using System;
using InventorySystem.Items;
using InventorySystem.Items.Core;
using InventorySystem.Items.ItemTypes;
using UnityEngine;

namespace InventorySystem.Core
{
    public interface IItemContainer
    {
        string Name { get; }
        GameObject Prefab { get; }
        Type Type { get; }
        IItemType ItemType { get; }
        int MaxItemsInStack { get; }
        ItemID ItemID { get; }
    }
}