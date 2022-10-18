using UnityEngine;

namespace UI.Inventory.Core
{
    public interface IDragDestination
    {
        int Index { get; }
        public void AddItems(Sprite itemSprite, int number, int index);
        void TryToSwap(IDragDestination inventorySlotUI, IInventoryHolder inventoryHolder);
    }
}