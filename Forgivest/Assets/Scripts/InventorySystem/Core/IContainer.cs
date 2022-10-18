using System;
using InventorySystem.Items;
using InventorySystem.Items.Core;

namespace InventorySystem.Core
{
    public interface IContainer
    {
        int Capacity { get; set; }
        bool IsFull { get; }

        IItem GetItem(Type itemType);
        Item[] GetAllItems();
        Item[] GetAllItems(Type itemType);
        Item[] GetEquippedItems();

        int GetItemAmount(Type itemType);
        bool TryToAdd(object sender, IItem item, int amount = 1, ItemContainer itemContainer = null);
        bool HasItem(Type type);
    }
}