using System;
using TMPro;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class NewSaveMenu : Core.Menu
    {
        [SerializeField] private Button _newSaveButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TMP_InputField _inputField;
        
        private string _saveFile = " ";

        public event Action<string> OnSaveClicked;

        private void OnEnable()
        {
            _newSaveButton.onClick.AddListener(OnSaveButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }
        
        private void OnDisable()
        {
            _newSaveButton.onClick.RemoveListener(OnSaveButtonClicked);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            
            _saveFile = string.Empty;
            _inputField.Select();
            _inputField.text = "";
        }
        
        public void CreateName(string saveFile)
        {
            _saveFile = saveFile;
        }

        private void OnBackButtonClicked()
        {
            MenuSwitcher.Show<SaveMenu>();
        }

        private void OnSaveButtonClicked()
        {
            OnSaveClicked?.Invoke(_saveFile);
        }
    }
}