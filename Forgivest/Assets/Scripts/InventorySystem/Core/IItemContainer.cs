using System;
using InventorySystem.Items;
using InventorySystem.Items.ItemTypes;

namespace InventorySystem.Core
{
    public interface IItemContainer
    {
        string Name { get; }
        Type Type { get; }
        IItemType ItemType { get; }
        int MaxItemsInStack { get; }
        ItemID ItemID { get; }
    }
}