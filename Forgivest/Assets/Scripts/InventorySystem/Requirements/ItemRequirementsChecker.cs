using System;
using System.Collections.Generic;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Core;
using LevelSystem.Scripts.Runtime;
using Zenject;

namespace InventorySystem.Requirements.Core
{
    public class ItemRequirementsChecker 
    {
        private readonly ItemContainer _inventory;
        private readonly ItemContainer _equipment;
        private readonly LevelController _levelController;

        public ItemRequirementsChecker(ItemContainer inventory,
            ItemContainer equipment, LevelController levelController)
        {
            _inventory = inventory;
            _equipment = equipment;
            _levelController = levelController;
        }

        public bool CheckRequirements(List<IRequirement> possibleSkillRequirements)
        {
            var isChecked = true;
            foreach (var possibleSkillRequirement in possibleSkillRequirements)
            {
                switch (possibleSkillRequirement)
                {
                    case ItemTypeRequirementsChecker itemTypeRequirementsChecker:
                        isChecked = HasItemTypeInContainer(itemTypeRequirementsChecker);
                        if (!isChecked)
                            return false;
                        break;
                    case ItemEquipmentRequirement itemRequirement:
                        isChecked = HasItemInContainer(itemRequirement.Item, ContainerType.Equipment);
                        if (!isChecked)
                            return false;
                        break;
                    case LevelRequirement levelRequirement:
                        isChecked = HasLevel(levelRequirement.NecessaryLevel);
                        if (!isChecked)
                            return false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(possibleSkillRequirement));
                }
            }

            return true;
        }

        private bool HasItemTypeInContainer(ItemTypeRequirementsChecker itemTypeRequirementsChecker)
        {
            foreach (var equipmentSlot in _equipment.Slots)
            {
                if(equipmentSlot.Item == null)
                    continue;
                
                if(equipmentSlot.Item.ItemType == itemTypeRequirementsChecker.ItemType)
                    return true;
            }

            return false;
        }

        public bool HasLevel(int level)
        {
            return _levelController.Level >= level;
        }

        public bool HasItemInContainer(Item item, ContainerType containerType)
        {
            return containerType switch
            {
                ContainerType.Inventory => _inventory.HasItem(item),
                ContainerType.Equipment => _equipment.HasItem(item),
                _ => throw new ArgumentOutOfRangeException(nameof(containerType), containerType, null)
            };
        }
    }
}