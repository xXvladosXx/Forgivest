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

        private void OnEnable()
        {
            _settingButton.onClick.AddListener(OnSettingsClicked);
            _startButton.onClick.AddListener(OnStartClicked);
        }

        private void OnStartClicked()
        {
            MenuSwitcher.Show<StartMenu>();
        }

        private void OnSettingsClicked()
        {
            MenuSwitcher.Show<SettingsMenu>();
        }

        private void OnDisable()
        {
            _settingButton.onClick.RemoveListener(OnSettingsClicked);
            _settingButton.onClick.RemoveListener(OnStartClicked);
        }

       /* public override void Initialize()
        {
            if (saveInteractor.GetLastSave == null)
            {
                _continueButton.interactable = false;
            }
            
            _startButton.onClick.AddListener(() => MenuSwitcher.Show<StartMenu>());
            _loadButton.onClick.AddListener(() => MenuSwitcher.Show<LoadMenu>());
            _settingButton.onClick.AddListener(() => MenuSwitcher.Show<SettingsMenu>());
            //_continueButton.onClick.AddListener(() => saveInteractor.ContinueGame(saveInteractor.GetLastSave));
            _quitButton.onClick.AddListener( () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
         Application.Quit();
            });
        }*/
    }

}
