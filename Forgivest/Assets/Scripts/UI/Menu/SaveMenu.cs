using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class SaveMenu: Core.Menu
    {
        [SerializeField] private string _warningText;

        [SerializeField] private Button _backButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _saveNewGameButton;
        [SerializeField] private Transform _content;
        
        private List<Button> _saveList = new List<Button>();

        public event Action<string> OnSaveClicked;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _saveNewGameButton.onClick.AddListener(OnNewSaveButtonClicked);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            _saveNewGameButton.onClick.RemoveListener(OnNewSaveButtonClicked);
        }
        
        public void Initialize(IEnumerable<string> savesList)
        {
            foreach (var save in savesList)
            {
                var button = Instantiate(_saveButton, _content);
                button.GetComponentInChildren<TextMeshProUGUI>().text = save;
                button.onClick.AddListener(() =>
                {
                    TryToSaveGame(save);  
                });    
                
                _saveList.Add(button);
            }
        }
        
        public void Clear()
        {
            foreach (var save in _saveList)
            {
                Destroy(save.gameObject);
            }
            
            _saveList.Clear();
        }

        private void TryToSaveGame(string save)
        {
            OnSaveClicked?.Invoke(save);
        }

        private void OnBackButtonClicked()
        {
            MenuSwitcher.Show<GameplayMenu>();
        }


        private void OnNewSaveButtonClicked()
        {
            MenuSwitcher.Show<NewSaveMenu>();
        }
    }
}