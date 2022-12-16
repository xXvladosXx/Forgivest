using System;
using InventorySystem.Requirements.Core;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu (menuName = "InventorySystem/Inventory")]
    public class Inventory : ScriptableObject
    {
        [field: SerializeField] public ItemContainer ItemContainer { get; private set; }
        [field: SerializeField] public int Capacity { get; private set; }

        public void Init(ItemRequirementsChecker itemRequirements)
        {
            ItemContainer.Init(Capacity, itemRequirements);
        }

        [ContextMenu("Swap item")]
        public void SwapItem()
        {
            ItemContainer.InitSlots();

            ItemContainer.DropItemIntoContainer(1, 0);
        }

        [ContextMenu("Generate slots")]
        public void GenerateSlot()
        {
            Init();
        }

        [ContextMenu("Generate slots by first slot example")]
        public void GenerateSlotsByFirstSlotExample()
        {
            var firstSlot = ItemContainer.Slots[0];
            ItemContainer.GenerateSlotsByFirstSlot(firstSlot, Capacity);
        }

        public void Init()
        {
            ItemContainer.Init(Capacity);
        }
    }
}