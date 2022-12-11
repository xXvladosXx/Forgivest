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

        [field: SerializeField] public Weapon StartWeapon { get; private set; }

        public GameObject CurrentColliderWeapon { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        public Amulet CurrentAmulet { get; private set; }
        public Back CurrentBack { get; private set; }
        public Boots CurrentBoots { get; private set; }
        public Chest CurrentChest { get; private set; }
        public Gloves CurrentGloves { get; private set; }
        public Bracer CurrentBracer { get; private set; }
        public Helmet CurrentHelmet { get; private set; }
        public Pants CurrentPants { get; private set; }
        public Ring CurrentRing { get; private set; }
        public Shoulder CurrentShoulder { get; private set; }
        
        public event Action<StatsableItem, bool> OnItemEquipped;
        public event Action<StatsableItem, bool> OnItemUnquipped;
        
        public event Action<Weapon> OnWeaponEquipped;


        public void Init()
        {
            TryToEquipWeapon(null);
        }

        public bool TryToEquip(StatsableItem statsableItem)
        {
            return statsableItem switch
            {
                Armor armor => TryToEquipArmor(armor),
                Weapon weapon => TryToEquipWeapon(weapon),
                _ => throw new ArgumentOutOfRangeException(nameof(statsableItem))
            };
        }

        public void Unequip(StatsableItem statsableItem)
        {
            switch (statsableItem)
            {
                case Armor armor:
                    UnequipArmor(armor);
                    break;
                case Weapon weapon:
                    UnequipWeapon(weapon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statsableItem));
            }
        }
        
        private bool TryToEquipArmor(Armor armor)
        {
            switch (armor)
            {
                case Amulet amulet:
                    CurrentAmulet = amulet;
                    break;
                case Back back:
                    CurrentBack = back;
                    break;
                case Boots boots:
                    CurrentBoots = boots;
                    break;
                case Bracer bracer:
                    CurrentBracer = bracer;
                    break;
                case Chest chest:
                    CurrentChest = chest;
                    break;
                case Gloves gloves:
                    CurrentGloves = gloves;
                    break;
                case Helmet helmet:
                    CurrentHelmet = helmet;
                    break;
                case Pants pants:
                    CurrentPants = pants;
                    break;
                case Ring ring:
                    CurrentRing = ring;
                    break;
                case Shoulder shoulder:
                    CurrentShoulder = shoulder;
                    break;
            }
            
            OnItemEquipped?.Invoke(armor, true);

            return true;
        }
        private bool UnequipArmor(Armor armor)
        {
            switch (armor)
            {
                case Amulet amulet:
                    CurrentAmulet = null;
                    break;
                case Back back:
                    CurrentBack = null;
                    break;
                case Boots boots:
                    CurrentBoots = null;
                    break;
                case Bracer bracer:
                    CurrentBracer = null;
                    break;
                case Chest chest:
                    CurrentChest = null;
                    break;
                case Gloves gloves:
                    CurrentGloves = null;
                    break;
                case Helmet helmet:
                    CurrentHelmet = null;
                    break;
                case Pants pants:
                    CurrentPants = null;
                    break;
                case Ring ring:
                    CurrentRing = null;
                    break;
                case Shoulder shoulder:
                    CurrentShoulder = null;
                    break;
            }
            
            OnItemUnquipped?.Invoke(armor, false);

            return true;
        }

        private bool TryToEquipWeapon(Weapon weapon)
        {
            if (weapon == null)
                return TryToEquipWeapon(StartWeapon);

            CurrentWeapon = weapon;
            CurrentColliderWeapon =
                Object.Instantiate(weapon.Prefab, weapon.RightHanded ? RightHand : LeftHand);
            CurrentColliderWeapon.layer = RightHand.gameObject.layer;
            CurrentColliderWeapon.GetComponent<Collider>().enabled = false; 
            
            OnItemEquipped?.Invoke(weapon, true);
            OnWeaponEquipped?.Invoke(weapon);
            
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

            TryToEquipWeapon(StartWeapon);
        }
    }
}