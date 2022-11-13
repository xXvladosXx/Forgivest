using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Stats
{
    public class ExperienceBarUI : UIElement
    {
        [SerializeField] private Image _experienceBar;
        
        private float _currentExperience;
        
        public void RecalculateExperience(float currentValue, float maxValue)
        {
            _currentExperience = currentValue;

            _experienceBar.fillAmount = _currentExperience / maxValue;
        }
    }
}