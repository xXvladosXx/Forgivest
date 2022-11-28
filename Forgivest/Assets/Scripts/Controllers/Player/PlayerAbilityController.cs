using System;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using Player;
using Requirements.Core;
using UI.Skill;
using Zenject;

namespace Controllers.Player
{
    public class PlayerAbilityController : IInitializable, ITickable, IDisposable
    {
        private readonly AbilityHandler _abilityHandler;
        private readonly IRequirementUser _requirementUser;
        private readonly SkillItemContainerUI _skillItemContainerUI;

        public PlayerAbilityController(AbilityHandler abilityHandler, SkillInventoryPanel skillInventoryPanel, PlayerEntity playerEntity)
        {
            _abilityHandler = abilityHandler;
            _requirementUser = playerEntity;
            _skillItemContainerUI = skillInventoryPanel.SkillItemContainerUI;
        }

        public void Initialize()
        {
            _skillItemContainerUI.CreateUpgradeButtons(_abilityHandler.AllAbilities.Capacity);
            FindLearnedSkills();

            OnPointsChanged(_abilityHandler.SkillPoints);
            _abilityHandler.OnPointsChanged += OnPointsChanged;
        }
        

        public void Tick()
        {
            for (int i = 0; i < _skillItemContainerUI.SkillCooldownRefreshers.Count; i++)
            {
                var ability = _abilityHandler.Hotbar.ItemContainer.Slots[i].Item as ActiveAbilityDefinition;
                if (ability == null)
                {
                    _skillItemContainerUI.SkillCooldownRefreshers[i].SetFillAmountToZero();
                    continue;
                }
                
                _skillItemContainerUI.SkillCooldownRefreshers[i]
                    .RefreshImage(_abilityHandler.GetCooldownOfAbility(ability.name), 
                        ability.Cooldown.DurationFormula.CalculateValue(_abilityHandler.gameObject));
            }
        }

        public void Dispose()
        {
            foreach (var abilityLearnButton in _skillItemContainerUI.SkillsToLearn)
            {
                abilityLearnButton.OnUpgradeClicked -= OnUpgradeClicked;
            }
            
            _abilityHandler.OnPointsChanged -= OnPointsChanged;
        }

        private void OnUpgradeClicked(int index)
        {
            var possibleSkill = _abilityHandler.AllAbilities.ItemContainer.Slots[index].Item as AbilityDefinition;
            
            if (possibleSkill == null) return;
            if (_abilityHandler.ItemContainer.ItemContainer.HasItem(possibleSkill)) return;
            
            var skillLearned = possibleSkill.RequirementsChecked(_requirementUser.LevelController.Level,
                _requirementUser.AbilityHandler.SkillPoints);
            
            if(!skillLearned) return;
            
            _skillItemContainerUI.SkillsToLearn[index].SkillLearned();
            _abilityHandler.ItemContainer.ItemContainer.TryToAddAtIndex(this, possibleSkill, 1, index);
            _abilityHandler.AddPoints(-possibleSkill.RequiredAbilityPoints);
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
                if (_abilityHandler.ItemContainer.ItemContainer.Slots[index].Item != null)
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