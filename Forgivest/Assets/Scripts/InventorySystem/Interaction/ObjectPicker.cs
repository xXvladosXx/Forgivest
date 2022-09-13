using System;

namespace InventorySystem.Interaction
{
    public class ObjectPicker
    {
        public void CollectObject(IPickable pickable)
        {
            switch (pickable)
            {
                case PickableItem pickableItem:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pickable));
            }
        }
    }
}