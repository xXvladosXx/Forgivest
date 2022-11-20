using System;
using InventorySystem.Items;
using InventorySystem.Items.Core;

namespace InventorySystem.Core
{
    public interface IContainer
    {
        int Capacity { get; set; }
        bool IsFull { get; }

        Item[] GetAllItems();

        bool TryToAdd(object sender, IItem item, int amount = 1, IContainer itemContainer = null);
    }
}