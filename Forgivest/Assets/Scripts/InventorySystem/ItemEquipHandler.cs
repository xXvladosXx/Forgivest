using System;
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

        public GameObject CurrentColliderWeapon { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        
        public bool TryToEquip(StatsableItem statsableItem)
        {
            switch (statsableItem)
            {
                case Armor armor:
                    return true;
                case Weapon weapon:
                    return TryToEquipWeapon(weapon);
                default:
                    throw new ArgumentOutOfRangeException(nameof(statsableItem));
            }
        }

        private bool TryToEquipWeapon(Weapon weapon)
        {
            if (CurrentColliderWeapon == null)
            {
                CurrentWeapon = weapon;
                CurrentColliderWeapon = Object.Instantiate(weapon.Prefab, weapon.RightHanded ? RightHand : LeftHand);
                return true;
            }

            Debug.Log("You have a weapon");

            return false;
        }
    }
}