using System;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu (menuName = "InventorySystem/Inventory")]
    public class Inventory : ScriptableObject
    {
        [field: SerializeField] public ItemContainer ItemContainer { get; private set; }
        [field: SerializeField] public int Capacity { get; private set; }

        [ContextMenu("Swap item")]
        public void SwapItem()
        {
            ItemContainer.InitSlots();

            ItemContainer.DropItemIntoContainer(0, 1);
        }

        private void OnValidate()
        {
        }

        public void Init()
        {
            ItemContainer.Init(Capacity);;
        }
    }
}