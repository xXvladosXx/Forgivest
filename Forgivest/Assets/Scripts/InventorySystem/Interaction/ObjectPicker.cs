using System;
using System.Collections.Generic;
using InventorySystem.Items;
using Sirenix.OdinInspector;
using StatsSystem.Core;
using StatsSystem.Core.Bonuses.Core;
using UnityEngine;

namespace InventorySystem.Interaction
{
    public class ObjectPicker : SerializedMonoBehaviour, IModifier
    {
        [field: SerializeField] public Inventory Inventory { get; private set; }
        
        [field: SerializeField] public PickableItem PickableItem { get; private set; }
        [field: SerializeField] public PickableItem PickableItem1 { get; private set; }
        
        private void Awake()
        {
            Inventory.Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Inventory.ItemContainer.TryToAdd(this, PickableItem.Item, 1);
            }
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                var  res =Inventory.ItemContainer.TryToAdd(this, PickableItem1.Item, 3);
                Debug.Log(res);
            }
        }

        public void CollectObject(IPickable pickable)
        {
            switch (pickable)
            {
                case PickableItem pickableItem:
                    Inventory.ItemContainer.TryToAdd(this, pickableItem.Item, pickableItem.Amount);
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