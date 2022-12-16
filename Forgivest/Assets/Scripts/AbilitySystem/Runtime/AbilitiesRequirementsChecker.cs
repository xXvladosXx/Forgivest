using System;
using System.Collections.Generic;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Core;
using InventorySystem.Requirements.Core;
using LevelSystem.Scripts.Runtime;

namespace AbilitySystem.AbilitySystem.Runtime
{
    public class AbilitiesRequirementsChecker
    {
        private readonly ItemContainer _abilityContainer;
        private readonly LevelController _levelController;
        private readonly AbilityHandler _abilityHandler;

        public AbilitiesRequirementsChecker(ItemContainer abilityContainer,  
            LevelController levelController, AbilityHandler abilityHandler)
        {
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
                        isChecked = HasItemInContainer(itemRequirement.Item, ContainerType.Skills);
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
                ContainerType.Skills => _abilityContainer.HasItem(item),
                _ => throw new ArgumentOutOfRangeException(nameof(containerType), containerType, null)
            };
        }
    }
}