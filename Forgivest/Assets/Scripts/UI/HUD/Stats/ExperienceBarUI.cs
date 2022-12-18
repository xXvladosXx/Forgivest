using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Stats
{
    public class ExperienceBarUI : UIElement
    {
        [SerializeField] private Image _experienceBar;
        [SerializeField] private TextMeshProUGUI _level;
        
        private float _currentExperience;
        
        public void RecalculateExperience(float currentValue, float maxValue, int level)
        {
            _currentExperience = currentValue;

            _experienceBar.fillAmount = _currentExperience / maxValue;
            
            _level.text = level.ToString();
        }
    }
}