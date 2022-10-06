﻿using System;
using System.Collections.Generic;
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
        }

        private void OnEnable()
        {
            ItemEquipHandler.OnItemEquipped += RecalculateStats;
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
                    Inventory.ItemContainer.TryToAdd(this, pickableItem.Item, pickableItem.Amount);
                    ItemEquipHandler.TryToEquip(pickableItem.Item as StatsableItem);
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


        /*public event Action<List<IBonus>> OnBonusAdded;
        public event Action<List<IBonus>> OnBonusRemoved;
        public List<IBonus> GetBonus()
        {
            var bonuses = new List<IBonus>();

            foreach (var weapon in Inventory.ItemContainer.GetAllItems())
            {
                if (weapon is IStatsable statsable)
                {
                    //bonuses.AddRange(statsable.StatsBonuses);
                    //bonuses.AddRange(statsable.CharacteristicBonuses);
                }
            }

            return bonuses;
        }*/
    }
}