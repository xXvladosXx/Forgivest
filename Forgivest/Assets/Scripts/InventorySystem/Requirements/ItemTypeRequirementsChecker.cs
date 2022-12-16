using InventorySystem.Items;
using InventorySystem.Items.ItemTypes.Core;
using UnityEngine;

namespace InventorySystem.Requirements.Core
{
    public class ItemTypeRequirementsChecker : IRequirement
    {
        [field: SerializeField] public ItemType ItemType { get; private set; }
        public string Description => $"Requires item type: {ItemType}";
    }
}