using System;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class SoundMenu : Core.Menu
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;
        [SerializeField] private Button _backButton;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClick);
        }

        private void OnBackButtonClick()
        {
            MenuSwitcher.Show<SettingsMenu>(false);   
        }
        
        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClick);
        }
    }
}