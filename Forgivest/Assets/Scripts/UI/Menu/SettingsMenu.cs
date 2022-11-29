using System;
using SoundSystem;
using TMPro;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI.Menu
{
    public class SettingsMenu : Core.Menu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _graphicsButton;
        
        private void OnEnable()
        {
            _soundButton.onClick.AddListener(ChangeSoundMenu);
            _graphicsButton.onClick.AddListener(ChangeGraphicsMenu);
            _backButton.onClick.AddListener(OnBackButton);
        }

        private void OnDisable()
        {
            _soundButton.onClick.RemoveListener(ChangeSoundMenu);
            _graphicsButton.onClick.RemoveListener(ChangeGraphicsMenu);
            _backButton.onClick.RemoveListener(OnBackButton);
        }

        private void ChangeGraphicsMenu()
        {
            MenuSwitcher.Show<GraphicsMenu>();
        }

        private void OnBackButton()
        {
            MenuSwitcher.Show<MainMenu>();
            MenuSwitcher.Show<GameplayMenu>();
        }

        private void ChangeSoundMenu()
        {
            MenuSwitcher.Show<SoundMenu>();
        }
    }
}