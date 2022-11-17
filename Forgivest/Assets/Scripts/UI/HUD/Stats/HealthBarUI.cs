using System.Globalization;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Stats
{
    public class HealthBarUI : UIElement
    {
        [SerializeField] private TextMeshProUGUI _healthValue;
        [SerializeField] private Image _health;
        
        private float _currentHealth;
        public void RecalculateHealth(float currentValue, float maxValue)
        {
            _currentHealth = currentValue;

            _health.fillAmount = _currentHealth / maxValue;
            _healthValue.text = $"{_currentHealth}/{maxValue}";
        }

        public void RecalculateHealth(float maxValue)
        {
            _health.fillAmount = _currentHealth / maxValue;
            _healthValue.text = $"{_currentHealth}/{maxValue}";
        }
    }
}