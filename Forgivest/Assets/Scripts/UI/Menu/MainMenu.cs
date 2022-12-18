using System;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MainMenu : Core.Menu
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingButton;
        [SerializeField] private Button _quitButton;
        public event Action OnContinueClick;

        private void OnEnable()
        {
            _settingButton.onClick.AddListener(OnSettingsClicked);
            _loadButton.onClick.AddListener(OnLoadClicked);
            _startButton.onClick.AddListener(OnStartClicked);
            _continueButton.onClick.AddListener(OnContinueClicked);
            _quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void OnDisable()
        {
            _settingButton.onClick.RemoveListener(OnSettingsClicked);
            _settingButton.onClick.RemoveListener(OnStartClicked);
            _settingButton.onClick.RemoveListener(OnLoadClicked);
            _continueButton.onClick.RemoveListener(OnContinueClicked);
            _quitButton.onClick.RemoveListener(OnQuitClicked);
        }

        private void OnContinueClicked()
        {
            OnContinueClick?.Invoke();
        }

        private void OnLoadClicked()
        {
            MenuSwitcher.Show<LoadMenu>();
        }

        private void OnStartClicked()
        {
            MenuSwitcher.Show<StartMenu>();
        }

        private void OnSettingsClicked()
        {
            MenuSwitcher.Show<SettingsMenu>();
        }

        private void OnQuitClicked()
        {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
        }
    }
}