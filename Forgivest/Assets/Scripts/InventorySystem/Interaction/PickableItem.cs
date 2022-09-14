using System;
using InventorySystem.Items;
using InventorySystem.Items.Core;
using UnityEngine;

namespace InventorySystem.Interaction
{
    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class PickableItem : MonoBehaviour, IPickable
    {
        [field: SerializeField] public Item Item { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }
}