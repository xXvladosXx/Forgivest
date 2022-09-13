using System;

namespace InventorySystem.Core
{
    public interface IItemContainer
    {
        string Name { get; }
        bool IsEquipped { get; set; }
        Type Type { get; }
        int MaxItemsInStack { get; }
        int Amount { get; set; }
    }
}