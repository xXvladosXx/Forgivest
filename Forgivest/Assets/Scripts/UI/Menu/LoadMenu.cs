using System;
using System.Text;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class LoadMenu : Core.Menu
    {
        [SerializeField] private string _warningText;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Transform _content;
        
        public event Action OnLoadClicked;


        private void OnEnable()
        {
            _loadButton.onClick.AddListener(TryToLoadGame);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnDisable()
        {
            _loadButton.onClick.RemoveListener(TryToLoadGame);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            
        }

        private void TryToLoadGame()
        {
            OnLoadClicked?.Invoke();
        }

        public override void Initialize()
        {
            
            /*foreach (var save in SaveInteractor.SaveList())
            {
                Button loadPrefab = Instantiate(_loadButton, _content);
                loadPrefab.GetComponentInChildren<TextMeshProUGUI>().text = save;
            
                loadPrefab.onClick.AddListener((() =>
                {
                    TryToLoad(save);
                }));
            }*/
        }

        private void TryToLoad(string save)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(_warningText).Append("<i>").Append($" {save}").Append("</i>").Append("?");
            
            /*WarningUI.Instance.ShowWarning(stringBuilder.ToString());
            WarningUI.Instance.OnAccepted += () => LoadGame(save);*/
        }

        private void LoadGame(string save)
        {
            //SaveInteractor.LoadGame(save);
        }
    }
}