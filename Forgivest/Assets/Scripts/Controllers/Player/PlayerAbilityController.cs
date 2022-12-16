using System;
using AbilitySystem.AbilitySystem.Runtime;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Core;
using InventorySystem.Items;
using InventorySystem.Requirements.Core;
using Player;
using UI.Skill;
using Zenject;

namespace Controllers.Player
{
    public class PlayerAbilityController : IInitializable, ITickable, IDisposable
    {
        private readonly AbilityHandler _abilityHandler;
        private readonly AbilitiesRequirementsChecker _abilitiesRequirementsChecker;
        private readonly SkillItemContainerUI _skillItemContainerUI;

        public PlayerAbilityController(AbilityHandler abilityHandler, SkillInventoryPanel skillInventoryPanel, PlayerEntity playerEntity)
        {
            _abilityHandler = abilityHandler;
            _abilitiesRequirementsChecker = playerEntity.AbilitiesRequirementsChecker;
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
            if (_abilityHandler.LearnedAbilities.ItemContainer.HasItem(possibleSkill)) return;

            var skillLearned = _abilitiesRequirementsChecker.CheckRequirements(possibleSkill.Requirements);
                
            if(!skillLearned) return;
            
            _skillItemContainerUI.SkillsToLearn[index].SkillLearned();
            _abilityHandler.LearnedAbilities.ItemContainer.TryToAddAtIndex(this, possibleSkill, 1, index);
            foreach (var requirement in possibleSkill.Requirements)
            {
                if (requirement is PointsRequirement pointsRequirement)
                {
                    _abilityHandler.AddPoints(-pointsRequirement.NecessaryPoints);
                    break;
                }
            }
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
                if (_abilityHandler.LearnedAbilities.ItemContainer.Slots[index].Item != null)
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