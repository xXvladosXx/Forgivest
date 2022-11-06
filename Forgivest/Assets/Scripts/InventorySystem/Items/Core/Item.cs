using System;
using InventorySystem.Core;
using InventorySystem.Items.ItemTypes.Core;
using InventorySystem.Items.Optional;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InventorySystem.Items.Core
{
    public abstract class Item : SerializedScriptableObject, IItem
    {
        [field: SerializeField] public Rarity Rarity { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: Min(1)]
        [field: SerializeField] public int MaxItemsInStack { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ItemID ItemID { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }
        public abstract string ItemDescription { get; protected set; }
        
        public string ColouredName()
        {
            string hexColour = ColorUtility.ToHtmlStringRGB(Rarity.Color);
            return $"<color=#{hexColour}>{Name}</color>";
        }
        
        public Type Type { get; }

        private void OnValidate()
        {
            if (MaxItemsInStack == 0)
            {
                MaxItemsInStack = 1;
            }
        }
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