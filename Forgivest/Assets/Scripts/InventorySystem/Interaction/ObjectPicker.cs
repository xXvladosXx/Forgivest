using System;
using System.Collections.Generic;
using InventorySystem.Items;
using RaycastSystem.Core;
using Sirenix.OdinInspector;
using StatsSystem.Core;
using StatsSystem.Core.Bonuses.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem.Interaction
{
    public class ObjectPicker : SerializedMonoBehaviour, IModifier
    {
        [field: SerializeField] public Inventory Inventory { get; private set; }
        [field: SerializeField] public ItemEquipHandler ItemEquipHandler { get; private set; }
        [field: SerializeField] public PickableItem PickableItem { get; private set; }
        [field: SerializeField] public PickableItem PickableItem1 { get; private set; }
        
        private RaycastUser _raycastUser;

        public void Init(RaycastUser raycastUser)
        {
            _raycastUser = raycastUser;
        }

        private void Awake()
        {
            Inventory.Init();
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

        public event Action<List<IBonus>> OnBonusAdded;
        public event Action<List<IBonus>> OnBonusRemoved;
        public List<IBonus> GetBonus()
        {
            var bonuses = new List<IBonus>();

            foreach (var weapon in Inventory.ItemContainer.GetAllItems())
            {
                if (weapon is IStatsable statsable)
                {
                    bonuses.AddRange(statsable.StatsBonuses);
                    bonuses.AddRange(statsable.CharacteristicBonuses);
                }
            }

            return bonuses;
        }

        
    }
}