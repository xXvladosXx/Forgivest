using System;
using System.Collections.Generic;
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
        [field: SerializeField] public GameObject ObjectToSpawn { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: Min(1)]
        [field: SerializeField] public int MaxItemsInStack { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ItemID ItemID { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField] public List<IRequirement> Requirements = new List<IRequirement>();

        [TextArea]
        [field: SerializeField] protected string _itemDescription; 
        public abstract string ItemDescription { get; }
        public abstract string ItemRequirements { get; }

        public string ColouredName()
        {
            if(Rarity == null) return Name;
            
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