using System;
using System.Text;
using TMPro;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class StartMenu : Core.Menu
    {
        [SerializeField] private string _warningText;
        [SerializeField] private string _sameGameWarningText;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _startNewGame;
        [SerializeField] private TMP_InputField _inputField;
        public event Action OnStartClicked;


        public void OnEnable()
        {
            _startNewGame.onClick.AddListener(TryToStartNewGame);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            MenuSwitcher.Show<MainMenu>();
        }

        private void TryToStartNewGame()
        {
            OnStartClicked?.Invoke();
            /*foreach (var saving in SaveInteractor.SaveList())
            {
                if (saving == SaveFile)
                {
                    stringBuilder.Clear();
                    stringBuilder.Append(_sameGameWarningText).Append("<i>").Append($" {SaveFile}").Append("</i>").Append("?");
                }
            }*/
            
            //WarningUI.Instance.ShowWarning(stringBuilder.ToString());
            
            //WarningUI.Instance.OnAccepted += StartNewGame;
        }

        public void CreateName(string saveFile)
        {
            SaveFile = saveFile;
        }

        private void OnDisable()
        {
            _startNewGame.onClick.RemoveListener(TryToStartNewGame);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
//            _inputField.text = string.Empty;
            SaveFile = string.Empty;
        }

        private void StartNewGame()
        {
            //WarningUI.Instance.OnAccepted -= StartNewGame;
            //SaveInteractor.StartNewGame(SaveFile);
        }
    }
}