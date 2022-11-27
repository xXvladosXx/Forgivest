using UI.Core;
using UI.Inventory;
using UI.Inventory.Core;
using UnityEngine;
using InventoryPanel = UI.Core.InventoryPanel;

namespace UI.Skill
{
    public class SkillInventoryPanel : InventoryPanel
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