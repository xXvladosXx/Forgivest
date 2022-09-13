using System;

namespace InventorySystem.Core
{
    public interface IContainer
    {
        int Capacity { get; set; }
        bool IsFull { get; }

        IItemContainer GetItem(Type itemType);
        IItemContainer[] GetAllItems();
        IItemContainer[] GetAllItems(Type itemType);
        IItemContainer[] GetEquippedItems();

        int GetItemAmount(Type itemType);
        bool TryToAdd(object sender, IItemContainer item);
        void Remove(object sender, Type itemType, int amount = 1);
        bool HasItem(Type type);
    }
}