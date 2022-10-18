using System;
using InventorySystem.Items;
using InventorySystem.Items.Armor;
using InventorySystem.Items.Weapon;
using StatSystem;
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

        public event Action<StatsableItem> OnItemEquipped;

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

        public void Unequip(StatsableItem statsableItem)
        {
            switch (statsableItem)
            {
                case Armor armor:
                    return;
                case Weapon weapon:
                    DeequipWeapon(weapon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statsableItem));
            }
        }

        private bool TryToEquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            CurrentColliderWeapon =
                Object.Instantiate(weapon.Prefab, weapon.RightHanded ? RightHand : LeftHand);
            OnItemEquipped?.Invoke(weapon);
            return true;
        }

        private void DeequipWeapon(Weapon weapon)
        {
            if (CurrentWeapon == null) return;
            if (CurrentWeapon != weapon) return;

            Object.Destroy(CurrentColliderWeapon.gameObject);
            CurrentWeapon = null;

            OnItemEquipped?.Invoke(null);
        }
    }
}