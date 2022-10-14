using UnityEngine;

namespace UI.Inventory.Core
{
    public interface IDragDestination
    {
        int Index { get; }
        int MaxAcceptable(Sprite item);
        public void AddItems(Sprite itemSprite, int number, int index);
        public void AddItems(Sprite itemSprite, int number);
        void TryToSwap(IDragDestination inventorySlotUI, Sprite imageSprite);
    }
}