using System;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class GameplayMenu : Core.Menu
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _mainMenuButton;

        public event Action OnContinueButtonClicked;
        public event Action OnMainMenuButtonClicked;
        
        private void OnEnable()
        {
            _continueButton.onClick.AddListener(OnContinueClicked);
            _loadButton.onClick.AddListener(OnLoadClicked);
            _saveButton.onClick.AddListener(OnSaveClicked);
            _settingsButton.onClick.AddListener(OnSettingsClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(OnContinueClicked);
            _loadButton.onClick.RemoveListener(OnLoadClicked);
            _saveButton.onClick.RemoveListener(OnSaveClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsClicked);
            _mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        }

        private void OnMainMenuClicked()
        {
            OnMainMenuButtonClicked?.Invoke();
        }

        private void OnSettingsClicked()
        {
            MenuSwitcher.Show<SettingsMenu>();
        }

        private void OnSaveClicked()
        {
            MenuSwitcher.Show<SaveMenu>();
        }

        private void OnLoadClicked()
        {
            MenuSwitcher.Show<LoadMenu>();
        }

        private void OnContinueClicked()
        {
            OnContinueButtonClicked?.Invoke();
        }
    }
}