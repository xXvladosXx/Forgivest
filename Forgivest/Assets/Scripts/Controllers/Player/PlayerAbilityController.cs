using System;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using Player;
using Requirements.Core;
using UI.Skill;
using Zenject;

namespace Controllers.Player
{
    public class PlayerAbilityController : IInitializable, ITickable, IDisposable
    {
        private readonly AbilityController _abilityController;
        private readonly IRequirementUser _requirementUser;
        private readonly SkillItemContainerUI _skillItemContainerUI;

        public PlayerAbilityController(AbilityController abilityController, SkillPanel skillPanel, PlayerEntity playerEntity)
        {
            _abilityController = abilityController;
            _requirementUser = playerEntity;
            _skillItemContainerUI = skillPanel.SkillItemContainerUI;
        }

        public void Initialize()
        {
            _skillItemContainerUI.CreateUpgradeButtons(_abilityController.AllAbilities.Capacity);
            FindLearnedSkills();

            OnPointsChanged(_abilityController.SkillPoints);
            _abilityController.OnPointsChanged += OnPointsChanged;
        }
        

        public void Tick()
        {
            for (int i = 0; i < _skillItemContainerUI.SkillCooldownRefreshers.Count; i++)
            {
                var ability = _abilityController.Hotbar.ItemContainer.Slots[i].Item as ActiveAbilityDefinition;
                if(ability == null) return;
                
                _skillItemContainerUI.SkillCooldownRefreshers[i]
                    .RefreshImage(_abilityController.GetCooldownOfAbility(ability.name), 
                        ability.Cooldown.DurationFormula.CalculateValue(_abilityController.gameObject));
            }
        }

        public void Dispose()
        {
            foreach (var abilityLearnButton in _skillItemContainerUI.SkillsToLearn)
            {
                abilityLearnButton.OnUpgradeClicked -= OnUpgradeClicked;
            }
        }

        private void OnUpgradeClicked(int index)
        {
            var possibleSkill = _abilityController.AllAbilities.ItemContainer.Slots[index].Item as AbilityDefinition;
            
            if (possibleSkill == null) return;
            if (_abilityController.ItemContainer.ItemContainer.HasItem(possibleSkill)) return;
            
            var skillLearned = possibleSkill.RequirementsChecked(_requirementUser.LevelController.Level,
                _requirementUser.AbilityController.SkillPoints);
            
            if(!skillLearned) return;
            
            _skillItemContainerUI.SkillsToLearn[index].SkillLearned();
            _abilityController.ItemContainer.ItemContainer.TryToAddAtIndex(this, possibleSkill, 1, index);
        }
        
        private void OnPointsChanged(int points)
        {
            _skillItemContainerUI.ChangeAbilityPoints(points);
        }
        
        private void FindLearnedSkills()
        {
            int index = 0;
            foreach (var abilityLearnButton in _skillItemContainerUI.SkillsToLearn)
            {
                if (_abilityController.ItemContainer.ItemContainer.Slots[index].Item != null)
                {
                    abilityLearnButton.SkillLearned();
                }
                else
                {
                    abilityLearnButton.OnUpgradeClicked += OnUpgradeClicked;
                }

                index++;
            }
        }
    }
}