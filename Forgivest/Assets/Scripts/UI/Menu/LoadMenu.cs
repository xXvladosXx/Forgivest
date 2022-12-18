using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class LoadMenu : Core.Menu
    {
        [SerializeField] private string _warningText;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Transform _content;
        
        private List<Button> _saveList = new List<Button>();

        public event Action<string> OnLoadClicked;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        public void Initialize(IEnumerable<string> savesList)
        {
            foreach (var save in savesList)
            {
                var button = Instantiate(_loadButton, _content);
                button.GetComponentInChildren<TextMeshProUGUI>().text = save;
                button.onClick.AddListener(() =>
                {
                    TryToLoadGame(save);  
                });    
                _saveList.Add(button);
            }
        }

        public void Dispose()
        {
            foreach (var button in _saveList)
            {
                button.onClick.RemoveAllListeners();
                Destroy(button.gameObject);
            }
            
            _saveList.Clear();
        }

        private void OnBackButtonClicked()
        {
            MenuSwitcher.Show<MainMenu>();
            MenuSwitcher.Show<GameplayMenu>();
        }

        private void TryToLoadGame(string save)
        {
            OnLoadClicked?.Invoke(save);
        }

        private void TryToLoad(string save)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(_warningText).Append("<i>").Append($" {save}").Append("</i>").Append("?");
            
            /*WarningUI.Instance.ShowWarning(stringBuilder.ToString());
            WarningUI.Instance.OnAccepted += () => LoadGame(save);*/
        }
    }
}