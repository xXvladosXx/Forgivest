using System;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using InventorySystem;
using InventorySystem.Items.Core;
using Zenject;

public class RequirementsChecker 
{
    private readonly ItemContainer _inventory;
    private readonly ItemContainer _equipment;
    private readonly ItemContainer _abilityContainer;

    public RequirementsChecker(ItemContainer inventory,
        ItemContainer equipment, ItemContainer abilityContainer)
    {
        _inventory = inventory;
        _equipment = equipment;
        _abilityContainer = abilityContainer;
    }

    public bool HasItemInAbilities(AbilityDefinition abilityDefinition)
    {
        return _abilityContainer.HasItem(abilityDefinition);
    }
    
    public bool HasItemInInventory(Item item)
    {
        return _inventory.HasItem(item);
    }

    public bool HasItemInEquipment(Item item)
    {
        return _equipment.HasItem(item);
    }
}