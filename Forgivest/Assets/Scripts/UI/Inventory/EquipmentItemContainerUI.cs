﻿using UI.Inventory.Core;
using UI.Inventory.Slot;
using UnityEngine;

namespace UI.Inventory
{
    public class EquipmentItemContainerUI : ItemContainerUI
    {
        [SerializeField] private InventorySlotUI[] _equipmentSlotUIs;
        
        protected override void CreateSlots(int inventoryCapacity)
        {
            foreach (var inventorySlot in _equipmentSlotUIs)
            {
                InventorySlotUis.Add(inventorySlot);
                inventorySlot.OnItemTryToSwap += OnItemTryToSwap;
            }
        }

        protected override void ClearSlots()
        {
            
        }
    }
}