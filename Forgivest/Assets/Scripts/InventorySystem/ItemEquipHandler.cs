using System;
using InventorySystem.Core;
using UnityEngine;

namespace InventorySystem
{
    public class ItemEquipHandler : MonoBehaviour
    {
        [field: SerializeField] public int Capacity { get; private set; }
        [field: SerializeField] public ItemContainer ItemContainer { get; private set; }

        public void AddItem()
        {
        }
    }
}