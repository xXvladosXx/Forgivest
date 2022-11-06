using System;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using InventorySystem;
using InventorySystem.Core;
using InventorySystem.Interaction;
using UI.Data;
using UI.HUD;
using UI.Inventory;
using UI.Inventory.Core;
using UI.Skill;
using UnityEngine;
using Zenject;

namespace Controllers.Player
{
    public class InventoryController : IInitializable, ITickable, IDisposable
    {
        private readonly InventoryPanel _inventoryPanel;
        private readonly SkillPanel _skillPanel;
        private readonly StaticPanel _staticPanel;
        private readonly ObjectPicker _objectPicker;
        private readonly AbilityHandler _abilityHandler;
        private readonly UIReusableData _uiReusableData;

        private Dictionary<ItemContainer, ItemContainerUI> _itemHolders =
            new Dictionary<ItemContainer, ItemContainerUI>();

        public InventoryController(InventoryPanel inventoryPanel, SkillPanel skillPanel, StaticPanel staticPanel,
            ObjectPicker objectPicker, AbilityHandler abilityHandler, UIReusableData uiReusableData)
        {
            _inventoryPanel = inventoryPanel;
            _skillPanel = skillPanel;
            _staticPanel = staticPanel;
            _objectPicker = objectPicker;
            _abilityHandler = abilityHandler;
            _uiReusableData = uiReusableData;
        }
        
        public void Initialize()
        {
            _inventoryPanel.InitializeInventory(_objectPicker.Inventory.Capacity);
            _inventoryPanel.InitializeEquipment(_objectPicker.Equipment.Capacity);
            _skillPanel.InitializeSkillInventory(_abilityHandler.AllAbilities.Capacity);
            _staticPanel.InitializeHotbarInventory(_objectPicker.Hotbar.Capacity);
            
            _itemHolders.Add(_objectPicker.Inventory.ItemContainer, _inventoryPanel.DynamicItemContainerUI);
            _itemHolders.Add(_objectPicker.Equipment.ItemContainer, _inventoryPanel.StaticItemContainerUI);
            _itemHolders.Add(_abilityHandler.AllAbilities.ItemContainer, _skillPanel.SkillItemContainerUI);
            _itemHolders.Add(_objectPicker.Hotbar.ItemContainer, _staticPanel.HotbarItemContainerUI);
            
            foreach (var itemContainer in _itemHolders.Keys)
            {
                RefreshSlotsData(itemContainer);
                itemContainer.OnItemAdded += OnItemAdded;
            }

            foreach (var itemContainer in _itemHolders.Values)
            {
                itemContainer.OnTryToSwapSlots += TryToSwapSlotsInInventory;
            }
        }
        
        public void Tick()
        {
            
        }

        public void Dispose()
        {
            foreach (var itemContainer in _itemHolders.Keys)
            {
                itemContainer.OnItemAdded -= OnItemAdded;
            }

            foreach (var itemContainer in _itemHolders.Values)
            {
                itemContainer.OnTryToSwapSlots -= TryToSwapSlotsInInventory;
            }
        }
        

        private void OnSwapCompleted(int source, int destination, ItemContainer itemContainer)
        {
            itemContainer.OnItemSwapped -= OnSwapCompleted;
            RefreshSlotsData(itemContainer);
        }
        
        private void TryToSwapSlotsInInventory(int source, int destination, Sprite sprite, int amount, IInventoryHolder sourceInventoryHolder)
        {
            if (sourceInventoryHolder != _uiReusableData.LastRaycastedInventoryHolder)
            {
                var destinationContainer = _itemHolders.FirstOrDefault(
                    x => x.Value == (ItemContainerUI) _uiReusableData.LastRaycastedInventoryHolder)
                    .Key;

                foreach (var itemContainer in _itemHolders)
                {
                    if (itemContainer.Value == (ItemContainerUI) _uiReusableData.LastRaycastedInventoryHolder) continue;
                    if (itemContainer.Value != (ItemContainerUI) sourceInventoryHolder) continue;
                    
                    itemContainer.Key.OnItemSwapped += OnSwapCompleted;
                    itemContainer.Key.DropItemIntoContainer(source,destination, destinationContainer);
                    itemContainer.Key.OnItemSwapped -= OnSwapCompleted;
                    RefreshSlotsData(destinationContainer);
                    return;
                }
            }
            
            foreach (var itemContainer in _itemHolders)
            {
                if (itemContainer.Value != (ItemContainerUI) sourceInventoryHolder) continue;

                itemContainer.Key.OnItemSwapped += OnSwapCompleted;
                itemContainer.Key.DropItemIntoContainer(source,destination);
                itemContainer.Key.OnItemSwapped -= OnSwapCompleted;
                        
                return;
            }
        }
        
        private void RefreshSlotsData(ItemContainer itemContainer)
        {
            int index = 0;
            foreach (var slot in itemContainer.Slots)
            {
                if (slot.Item == null)
                {
                    _inventoryPanel.CreateSlotsWithItemInInventory(null, 0, index,
                        _itemHolders[itemContainer], itemContainer.Slots[index].GetDescription(),
                        itemContainer.Slots[index].GetItemName(),
                        itemContainer.Slots[index].GetRequirements());

                    index++;
                    continue;
                }

                _inventoryPanel.CreateSlotsWithItemInInventory(slot.Item.Sprite, slot.Amount, index,
                    _itemHolders[itemContainer], itemContainer.Slots[index].GetDescription(),
                    itemContainer.Slots[index].GetItemName(),
                    itemContainer.Slots[index].GetRequirements());

                index++;
            }
        }
        
        private void OnItemAdded(object sender, IItem item, int amount, ItemContainer itemContainer)
        {
            RefreshSlotsData(itemContainer);
        }
    }
}