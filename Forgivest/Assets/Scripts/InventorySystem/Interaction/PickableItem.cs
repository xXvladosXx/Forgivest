using System;
using InventorySystem.Items;
using UnityEngine;

namespace InventorySystem.Interaction
{
    [Serializable]
    public class PickableItem : IPickable
    {
        [field: SerializeField] public Item Item { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }
}