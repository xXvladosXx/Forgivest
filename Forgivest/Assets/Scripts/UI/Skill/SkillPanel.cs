using UI.Core;
using UI.Inventory;
using UI.Inventory.Core;
using UnityEngine;

namespace UI.Skill
{
    public class SkillPanel : Panel
    {
        [SerializeField] private SkillItemContainerUI _skillItemContainerUI;
    
        public SkillItemContainerUI SkillItemContainerUI => _skillItemContainerUI;
        
        private void OnEnable()
        {
            _skillItemContainerUI.OnInventoryHolderChanged += ChangeHolder;
        }

        private void OnDisable()
        {
            _skillItemContainerUI.OnInventoryHolderChanged -= ChangeHolder;
        }

        public void InitializeSkillInventory(int hotbarCapacity)
        {
            _skillItemContainerUI.InitializeSlots(hotbarCapacity);
        }
    }
}