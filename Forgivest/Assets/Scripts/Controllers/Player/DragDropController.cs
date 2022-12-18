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
using UnityEngine.InputSystem;
using Utilities;
using Zenject;
using Object = UnityEngine.Object;

namespace Controllers.Player
{
    public class DragDropController : IInitializable, ITickable, IDisposable
    {
        private readonly InventoryPanel _inventoryPanel;
        private readonly SkillInventoryPanel _skillInventoryPanel;
        private readonly StaticInventoryPanel _staticInventoryPanel;
        private readonly ObjectPicker _objectPicker;
        private readonly AbilityHandler _abilityHandler;
        private readonly UIReusableData _uiReusableData;
        private readonly PlayerInputProvider _playerInputProvider;

        private readonly Dictionary<ItemContainer, ItemContainerUI> _itemHolders =
            new Dictionary<ItemContainer, ItemContainerUI>();

        public DragDropController(InventoryPanel inventoryPanel, SkillInventoryPanel skillInventoryPanel, StaticInventoryPanel staticInventoryPanel,
            ObjectPicker objectPicker, AbilityHandler abilityHandler, UIReusableData uiReusableData)
        {
            _inventoryPanel = inventoryPanel;
            _skillInventoryPanel = skillInventoryPanel;
            _staticInventoryPanel = staticInventoryPanel;
            _objectPicker = objectPicker;
            _abilityHandler = abilityHandler;
            _uiReusableData = uiReusableData;
        }
        
        public void Initialize()
        {
            _inventoryPanel.InitializeInventory(_objectPicker.Inventory.Capacity);
            _inventoryPanel.InitializeEquipment(_objectPicker.Equipment.Capacity);
            _skillInventoryPanel.InitializeSkillInventory(_abilityHandler.AllAbilities.Capacity);
            _staticInventoryPanel.InitializeHotbarInventory(_objectPicker.Hotbar.Capacity);
            
            _itemHolders.Add(_objectPicker.Inventory.ItemContainer, _inventoryPanel.DynamicItemContainerUI);
            _itemHolders.Add(_objectPicker.Equipment.ItemContainer, _inventoryPanel.StaticItemContainerUI);
            _itemHolders.Add(_abilityHandler.AllAbilities.ItemContainer, _skillInventoryPanel.SkillItemContainerUI);
            _itemHolders.Add(_objectPicker.Hotbar.ItemContainer, _staticInventoryPanel.HotbarItemContainerUI);
            
            foreach (var itemContainer in _itemHolders.Keys)
            {
                RefreshSlotsData(itemContainer);
                itemContainer.OnItemAdded += OnItemAdded;
                itemContainer.OnSlotsChanged += RefreshSlotsData;
            }

            foreach (var itemContainer in _itemHolders.Values)
            {
                itemContainer.OnTryToSwapSlots += TryToSwapSlotsInInventory;
                itemContainer.OnTryToDropSlot += TryToDropSlotFromInventory;
            }
        }

        private void OnSlotsChanged()
        {
            
        }

        public void Tick()
        {
            
        }

        public void Dispose()
        {
            foreach (var itemContainer in _itemHolders.Keys)
            {
                itemContainer.OnItemAdded -= OnItemAdded;
                itemContainer.OnSlotsChanged -= RefreshSlotsData;
            }

            foreach (var itemContainer in _itemHolders.Values)
            {
                itemContainer.OnTryToSwapSlots -= TryToSwapSlotsInInventory;
                itemContainer.OnTryToDropSlot -= TryToDropSlotFromInventory;
            }
        }

        private void TryToDropSlotFromInventory(int source, Sprite sprite, int amount, IInventoryHolder sourceInventoryHolder)
        {
            if(_uiReusableData.LastRaycastedInventoryHolder != null) return;
            
            foreach (var itemHolder in _itemHolders)
            {
                if (itemHolder.Value == (ItemContainerUI)sourceInventoryHolder)
                {
                    if (!itemHolder.Key.Slots[source].Changeable)
                    {
                        return;
                    }

                    var prefab = itemHolder.Key.GetItemAtIndex(source).ObjectToSpawn;
                    if (prefab)
                    {
                        Object.Instantiate(prefab, _objectPicker.PossiblePointToSpawn(), Quaternion.Euler(60, 0, 75));
                    }
                        
                    itemHolder.Key.Remove(this, source, true, amount);
                    RefreshSlotsData(itemHolder.Key);
                }
            }
        }


        private void TryToSwapSlotsInInventory(int source, int destination, Sprite sprite, int amount, IInventoryHolder sourceInventoryHolder)
        {
            if (sourceInventoryHolder != _uiReusableData.LastRaycastedInventoryHolder)
            {
                var destinationContainer = _itemHolders.FirstOrDefault(
                    x => 
                        x.Value == (ItemContainerUI) _uiReusableData.LastRaycastedInventoryHolder)
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

        private void OnSwapCompleted(int source, int destination, ItemContainer itemContainer)
        {
            itemContainer.OnItemSwapped -= OnSwapCompleted;
            RefreshSlotsData(itemContainer);
        }

        private void RefreshSlotsData(ItemContainer itemContainer)
        {
            int index = 0;
            foreach (var slot in itemContainer.Slots)
            {
                if (slot.Item == null)
                {
                    _inventoryPanel.RefreshSlotsWithItemInInventory(null, 0, index,
                        _itemHolders[itemContainer], itemContainer.Slots[index].GetDescription(),
                        itemContainer.Slots[index].GetItemName(),
                        itemContainer.Slots[index].GetRequirements());

                    index++;
                    continue;
                }

                _inventoryPanel.RefreshSlotsWithItemInInventory(slot.Item.Sprite, slot.Amount, index,
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