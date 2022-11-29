using System;
using System.Collections.Generic;
using TMPro;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI.Menu
{
    public class GraphicsMenu : Core.Menu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private TMP_Dropdown _quality;
        [SerializeField] private TMP_Dropdown _resolutionsDropdown;
        [SerializeField] private Toggle _fullScreen;

        private Resolution[] _resolutions;
        private bool _fullScreened;
        private int _graphicsIndex;
        private int _resolutionIndex;

        public event Action<int, int, int> OnGraphicsSettingsChanged;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButton);
            _fullScreen.onValueChanged.AddListener(SetFullscreen);
            _quality.onValueChanged.AddListener(SetQualityLevelDropdown);
            _resolutionsDropdown.onValueChanged.AddListener(SetResolution);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButton);
            _fullScreen.onValueChanged.RemoveListener(SetFullscreen);
            _quality.onValueChanged.RemoveListener(SetQualityLevelDropdown);
            _resolutionsDropdown.onValueChanged.RemoveListener(SetResolution);
        }

        public void Init()
        {
            _resolutions = new Resolution[Screen.resolutions.Length];
            _resolutions = Screen.resolutions;
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            _resolutionIndex = resolutionIndex;
            OnGraphicsSettingsChanged?.Invoke(resolutionIndex, Convert.ToInt32(_fullScreened), _graphicsIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;

            _fullScreened = isFullscreen;
            OnGraphicsSettingsChanged?.Invoke(_resolutionIndex, Convert.ToInt32(_fullScreened), _graphicsIndex);
        }

        public void SetQualityLevelDropdown(int index)
        {
            QualitySettings.SetQualityLevel(index, false);
            _quality.value = index;
            _graphicsIndex = index;
            
            OnGraphicsSettingsChanged?.Invoke(_resolutionIndex, Convert.ToInt32(_fullScreened), _graphicsIndex);
        }

        private void OnBackButton()
        {
            MenuSwitcher.Show<MainMenu>();
        }

        public void FindResolution(int resolution)
        {
            _resolutionsDropdown.ClearOptions();
            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);

                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            _resolutionsDropdown.AddOptions(options);
            _resolutionsDropdown.value = currentResolutionIndex;
            _resolutionsDropdown.RefreshShownValue();

            _resolutionsDropdown.value = resolution;
            _resolutionsDropdown.RefreshShownValue();
            SetResolution(_resolutionsDropdown.value);
        }
    }
}