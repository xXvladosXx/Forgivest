using System;
using System.Collections.Generic;
using InventorySystem.Core;
using InventorySystem.Items;
using RaycastSystem.Core;
using Sirenix.OdinInspector;
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
        public event Action<List<StatModifier>> OnStatChanged;

        public void Init(RaycastUser raycastUser)
        {
            _raycastUser = raycastUser;
        }

        private void Awake()
        {
            Inventory.Init();
            Equipment.Init();
            Hotbar.Init();
        }

        private void OnEnable()
        {
            ItemEquipHandler.OnItemEquipped += RecalculateStats;
            Equipment.ItemContainer.OnItemAdded += OnItemEquipped;
            Equipment.ItemContainer.OnItemRemoved += OnItemRemoved;
        }
        

        private void OnDisable()
        {
            ItemEquipHandler.OnItemEquipped -= RecalculateStats;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPickable pickable))
            {
                CollectObject(pickable);
            }
        }

        public void CollectObject(IPickable pickable)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(pickable));
            }
        }
        
        private void RecalculateStats(StatsableItem statsableItem)
        {
            var modifiers = new List<StatModifier>();
            
            foreach (var weapon in Inventory.ItemContainer.GetAllItems())
            {
                if (weapon is IStatsable statsable)
                {
                    modifiers.AddRange(statsable.StatModifier);
                }
            }
            
            OnStatChanged?.Invoke(modifiers);
        }
        
        private void OnItemRemoved(object arg1, IItem arg2, int arg3, ItemContainer arg4)
        {
            ItemEquipHandler.Unequip((StatsableItem)arg2);
        }

        private void OnItemEquipped(object arg1, IItem arg2, int arg3, ItemContainer arg4)
        {
            ItemEquipHandler.TryToEquip((StatsableItem)arg2);
        }
    }
}