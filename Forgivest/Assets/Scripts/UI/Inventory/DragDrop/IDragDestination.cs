using UI.Inventory.Core;

namespace UI.Inventory.DragDrop
{
    public interface IDragDestination
    {
        int Index { get; }
        void TryToSwap(IDragDestination inventorySlotUI, IInventoryHolder inventoryHolder);
    }
}