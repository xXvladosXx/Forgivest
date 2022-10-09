using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.Items.Core;
using UnityEditor;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(menuName = "InventorySystem/ItemDatabase")]
    public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        [field: SerializeField] public Item[] InventoryItems { get; private set; }
        private Dictionary<int, Item> _idItems = new Dictionary<int, Item>();

        private void OnValidate()
        {
            UpdateID();
        }

        public void UpdateID()
        {
            _idItems.Clear();
            
            for (int i = 0; i < InventoryItems.Length; i++)
            {
                var itemID = InventoryItems[i].ItemID;
                if (itemID != null && itemID.Id != i)
                {
                    InventoryItems[i].ItemID.Id = i;
                }
                _idItems.Add(i, InventoryItems[i]);
            }
        }
        
        public Item GetItemByID(int id)
        {
            _idItems.TryGetValue(id, out var item);

            return item;
        }

        public void OnBeforeSerialize()
        {
            UpdateID();
        }

        public void OnAfterDeserialize()
        {
            UpdateID();
        }
    }
}