using System;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Skill
{
    public class AbilityLearnButton : UIElement
    {
        [SerializeField] private Button _skillLearnButton;
        [SerializeField] private Image _image;

        private int _index;
        
        public event Action<int> OnUpgradeClicked;
        
        public void Init(int index, Sprite icon)
        {
            _index = index;
            _image.sprite = icon;
        }
        
        private void OnEnable()
        {
            _skillLearnButton.onClick.AddListener(TryToUpgradeSkill);
        }

        private void OnDisable()
        {
            _skillLearnButton.onClick.RemoveListener(TryToUpgradeSkill);
        }
        
        public void SkillLearned()
        {
            _skillLearnButton.gameObject.SetActive(false);
        }

        private void TryToUpgradeSkill()
        {
            OnUpgradeClicked?.Invoke(_index);
        }
    }
}