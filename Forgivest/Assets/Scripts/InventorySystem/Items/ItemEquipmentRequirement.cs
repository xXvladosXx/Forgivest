using System;
using InventorySystem.Items.Core;
using UnityEngine;

namespace InventorySystem.Items
{
    [Serializable]
    public class ItemEquipmentRequirement : IRequirement
    {
        [field: SerializeField] public Item Item { get; private set; }
        public string Description => $"Requires item: {Item.Name}";
    }
}