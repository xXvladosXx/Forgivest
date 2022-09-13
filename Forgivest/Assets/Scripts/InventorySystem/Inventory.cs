using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu (menuName = "InventorySystem/Inventory")]
    public class Inventory : ScriptableObject
    {
        [field: SerializeField] public ItemContainer ItemContainer { get; private set; }
        [field: SerializeField] public int Capacity { get; private set; }
        
        public void Init()
        {
            ItemContainer.Capacity = Capacity;
        }
    }
}