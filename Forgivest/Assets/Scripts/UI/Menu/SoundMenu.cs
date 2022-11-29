using System;
using SoundSystem;
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

        public event Action<float, float> OnAudioSettingsChanged; 

        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClick);
            _musicSlider.onValueChanged.AddListener(SaveSettings);
            _effectsSlider.onValueChanged.AddListener(SaveSettings);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClick);
            _musicSlider.onValueChanged.RemoveListener(SaveSettings);
            _effectsSlider.onValueChanged.RemoveListener(SaveSettings);
        }

        public void LoadAudioSettings(SettingsData loadSettings)
        {
            _musicSlider.value = loadSettings.MusicVolume;
            _effectsSlider.value = loadSettings.EffectsVolume;
        }

        private void OnBackButtonClick()
        {
            MenuSwitcher.Show<SettingsMenu>(false);   
        }

        private void SaveSettings(float value)
        {
            OnAudioSettingsChanged?.Invoke(_musicSlider.value, _effectsSlider.value);
        }
    }
}