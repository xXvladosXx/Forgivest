using System;
using InventorySystem.Core;
using InventorySystem.Items;
using InventorySystem.Items.Armor;
using InventorySystem.Items.Weapon;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InventorySystem
{
    [Serializable]
    public class ItemEquipHandler 
    {
        [field: SerializeField] public Transform RightHand { get; private set; }
        [field: SerializeField] public Transform LeftHand { get; private set; }

        private GameObject _currentWeapon;
        
        public void Equip(StatsableItem statsableItem)
        {
            switch (statsableItem)
            {
                case Armor armor:
                    break;
                case Weapon weapon:
                    EquipWeapon(weapon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statsableItem));
            }
        }

        private void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = Object.Instantiate(weapon.Prefab, weapon.RightHanded ? RightHand : LeftHand);
        }
    }
}