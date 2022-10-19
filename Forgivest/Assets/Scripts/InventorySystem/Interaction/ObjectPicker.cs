using System;
using System.Collections.Generic;
using InventorySystem.Core;
using InventorySystem.Items;
using InventorySystem.Items.ItemTypes.Core;
using InventorySystem.Items.Weapon;
using RaycastSystem.Core;
using Sirenix.OdinInspector;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using UnityEngine;

namespace InventorySystem.Interaction
{
    public class ObjectPicker : SerializedMonoBehaviour, IStatsChangeable
    {
        [field: SerializeField] public Inventory Inventory { get; private set; }
        [field: SerializeField] public Inventory Equipment { get; private set; }
        [field: SerializeField] public Inventory Hotbar { get; private set; }

        [field: SerializeField] public ItemEquipHandler ItemEquipHandler { get; private set; }
        [field: SerializeField] public PickableItem PickableItem { get; private set; }
        [field: SerializeField] public PickableItem PickableItem1 { get; private set; }

        private RaycastUser _raycastUser;
        public event Action<List<StatModifier>> OnStatAdded;
        public event Action<List<StatModifier>> OnStatRemoved;

        public void Init(RaycastUser raycastUser)
        {
            _raycastUser = raycastUser;
            Inventory.Init();
            Equipment.Init();
            Hotbar.Init();
        }

        private void Start()
        {
            ItemEquipHandler.TryToEquip(Equipment.ItemContainer.GetItem(ItemType.Sword) as StatsableItem);
        }

        private void OnEnable()
        {
            ItemEquipHandler.OnItemEquipped += RecalculateStats;
            ItemEquipHandler.OnItemUnquipped += RecalculateStats;
            Equipment.ItemContainer.OnItemAdded += OnItemEquipped;
            Equipment.ItemContainer.OnItemRemoved += OnItemRemoved;
        }

        private void OnDisable()
        {
            ItemEquipHandler.OnItemEquipped -= RecalculateStats;
            ItemEquipHandler.OnItemUnquipped -= RecalculateStats;
            Equipment.ItemContainer.OnItemAdded -= OnItemEquipped;
            Equipment.ItemContainer.OnItemRemoved -= OnItemRemoved;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPickable pickable))
            {
                CollectObject(pickable);
            }
        }

        private void CollectObject(IPickable pickable)
        {
            switch (pickable)
            {
                case PickableItem pickableItem:
                    var equipped = Equipment.ItemContainer.TryToAdd(this, pickableItem.Item, pickableItem.Amount);
                    if (!equipped)
                    {
                        Inventory.ItemContainer.TryToAdd(this, pickableItem.Item, pickableItem.Amount);
                    }
                    break;
            }
        }
        
        private void RecalculateStats(StatsableItem statsableItem, bool adding)
        {
            if (adding)
            {
                OnStatAdded?.Invoke(statsableItem.StatModifier);
            }
            else
            {
                OnStatRemoved?.Invoke(statsableItem.StatModifier);
            }
        }
        
        
        private void OnItemRemoved(object sender, IItem item, int amount, ItemContainer itemContainer)
        {
            ItemEquipHandler.Unequip((StatsableItem)item);
        }

        private void OnItemEquipped(object sender, IItem item, int amount, ItemContainer itemContainer)
        {
            ItemEquipHandler.TryToEquip((StatsableItem)item);
        }
    }
}