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
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
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

        private void TryToSaveGame(string save)
        {
            OnSaveClicked?.Invoke(save);
        }

        private void OnBackButtonClicked()
        {
            MenuSwitcher.Show<GameplayMenu>();
        }


        protected void OnWEnable()
        {
            /*foreach (var save in SaveInteractor.SaveList())
            {
                Button savePrefab = Instantiate(_saveButton, _content);
                savePrefab.GetComponentInChildren<TextMeshProUGUI>().text = save;
            
                savePrefab.onClick.AddListener( ()=>
                {
                    TryToSave(save);
                });
            }*/
        }

        private void TryToSave(string save)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(_warningText).Append("<i>").Append($" {save}").Append("</i>").Append("?");
            
            /*WarningUI.Instance.ShowWarning(stringBuilder.ToString());
            WarningUI.Instance.OnAccepted += () => Save(save);*/
        }

        private void Save(string save)
        {
            //SaveInteractor.Save(save);
        }
    }
}