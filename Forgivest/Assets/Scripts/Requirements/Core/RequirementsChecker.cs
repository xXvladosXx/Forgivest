using System;
using System.Collections.Generic;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Core;
using LevelSystem.Scripts.Runtime;
using Zenject;

public class RequirementsChecker 
{
    private readonly ItemContainer _inventory;
    private readonly ItemContainer _equipment;
    private readonly ItemContainer _abilityContainer;
    private readonly AbilityHandler _abilityHandler;
    private readonly LevelController _levelController;

    public RequirementsChecker(ItemContainer inventory,
        ItemContainer equipment, ItemContainer abilityContainer, 
        LevelController levelController, AbilityHandler abilityHandler)
    {
        _inventory = inventory;
        _equipment = equipment;
        _abilityContainer = abilityContainer;
        _levelController = levelController;
        _abilityHandler = abilityHandler;
    }

    public bool CheckRequirements(List<IRequirement> possibleSkillRequirements)
    {
        var isChecked = true;
        
        foreach (var possibleSkillRequirement in possibleSkillRequirements)
        {
            switch (possibleSkillRequirement)
            {
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
                case PointsRequirement pointsRequirement:
                    isChecked = HasEnoughPointsToLearnSkill(pointsRequirement.NecessaryPoints);
                    if (!isChecked)
                        return false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(possibleSkillRequirement));
            }
        }

        return true;
    }

    public bool HasLevel(int level)
    {
        return _levelController.Level >= level;
    }

    public bool HasEnoughPointsToLearnSkill(int points)
    {
        return _abilityHandler.SkillPoints >= points; 
    }

    public bool HasItemInContainer(Item item, ContainerType containerType)
    {
        return containerType switch
        {
            ContainerType.Inventory => _inventory.HasItem(item),
            ContainerType.Equipment => _equipment.HasItem(item),
            ContainerType.Skills => _abilityContainer.HasItem(item),
            _ => throw new ArgumentOutOfRangeException(nameof(containerType), containerType, null)
        };
    }
}

public enum ContainerType
{
    Inventory,
    Equipment,
    Skills
}