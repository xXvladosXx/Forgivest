using System;
using InventorySystem.Core;
using InventorySystem.Items.ItemTypes;
using InventorySystem.Items.ItemTypes.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InventorySystem.Items.Core
{
    public abstract class Item : SerializedScriptableObject, IItem
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public int MaxItemsInStack { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ItemID ItemID { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }
        public Type Type { get; }
    }
    
    [Serializable]
    public class ItemID
    {
        public int Id = -1;

        public ItemID()
        {
            Id = -1;
        }
        public ItemID(int id)
        {
            Id = id;
        }
    }
}