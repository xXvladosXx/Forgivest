using System;
using InventorySystem.Core;
using UnityEngine;

namespace InventorySystem.Items
{
    public abstract class Item : ScriptableObject, IItemContainer
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public bool IsEquipped { get; set; }
        [field: SerializeField] public int MaxItemsInStack { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        public Type Type { get; }
        public int Amount { get; set; }
        
    }
}