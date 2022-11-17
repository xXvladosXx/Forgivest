using System.Collections.Generic;
using TMPro;
using UI.Inventory;
using UnityEngine;

namespace UI.Skill
{
    public class SkillItemContainerUI : StaticItemContainerUI
    {
        [field: SerializeField] public List<AbilityLearnButton> SkillsToLearn { get; private set; } = new List<AbilityLearnButton>();
        [field: SerializeField] public List<AbilityCooldownRefresher> SkillCooldownRefreshers { get; private set; } = new List<AbilityCooldownRefresher>();
        [field: SerializeField] public TextMeshProUGUI AbilityPointsText { get; private set; }
        
        public void ChangeAbilityPoints(int amount)
        {
            AbilityPointsText.text = amount.ToString();
        }
        
        public void CreateUpgradeButtons(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                SkillsToLearn[i].Init(i, _equipmentSlotUIs[i].GetIcon());
            }
        }
    }
}