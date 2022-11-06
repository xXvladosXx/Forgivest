using System;
using InventorySystem.Items.Core;
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