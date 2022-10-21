using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Stats
{
    public class ManaBarUI : UIElement
    {
        [SerializeField] private TextMeshProUGUI _manaValue;
        [SerializeField] private Image _mana;

        private float _currentMana;

        public void RecalculateMana(float currentValue, float maxValue)
        {
            _currentMana = currentValue;

            _mana.fillAmount = _currentMana / maxValue;
            _manaValue.text = $"{_currentMana}/{maxValue}";
        }

        public void RecalculateMana(float maxValue)
        {
            _mana.fillAmount = _currentMana / maxValue;
            _manaValue.text = $"{_currentMana}/{maxValue}";
        }
    }
}