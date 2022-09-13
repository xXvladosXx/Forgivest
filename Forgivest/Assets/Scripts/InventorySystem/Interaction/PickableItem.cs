using InventorySystem.Items;
using UnityEngine;

namespace InventorySystem.Interaction
{
    public class PickableItem : IPickable
    {
        [field: SerializeField] public Item Item { get; private set; }
        
        public void Pick()
        {
            
        }
    }
}