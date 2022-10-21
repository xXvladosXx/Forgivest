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

        [field: SerializeField] public Weapon StartWeapon { get; private set; }

        public GameObject CurrentColliderWeapon { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        public event Action<StatsableItem, bool> OnItemEquipped;
        public event Action<StatsableItem, bool> OnItemUnquipped;
        
        public event Action<Weapon> OnWeaponEquipped;


        public void Init()
        {
            TryToEquipWeapon(null);
        }

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
                    UnequipWeapon(weapon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statsableItem));
            }
        }

        private bool TryToEquipWeapon(Weapon weapon)
        {
            if (weapon == null)
                return EquipStartWeapon();

            CurrentWeapon = weapon;
            CurrentColliderWeapon =
                Object.Instantiate(weapon.Prefab, weapon.RightHanded ? RightHand : LeftHand);
            CurrentColliderWeapon.layer = RightHand.gameObject.layer;
            
            OnItemEquipped?.Invoke(weapon, true);
            OnWeaponEquipped?.Invoke(weapon);
            
            return true;
        }

        private bool EquipStartWeapon()
        {
            CurrentWeapon = StartWeapon;
            CurrentColliderWeapon =
                Object.Instantiate(StartWeapon.Prefab, StartWeapon.RightHanded ? RightHand : LeftHand);

            OnItemEquipped?.Invoke(StartWeapon, true);
            OnWeaponEquipped?.Invoke(StartWeapon);

            return true;
        }

        private void UnequipWeapon(Weapon weapon)
        {
            if (CurrentWeapon == null) return;
            if (CurrentWeapon == StartWeapon) return;
            if (CurrentWeapon != weapon) return;

            Object.Destroy(CurrentColliderWeapon.gameObject);
            CurrentWeapon = null;

            OnItemUnquipped?.Invoke(weapon, false);

            EquipStartWeapon();
        }
    }
}