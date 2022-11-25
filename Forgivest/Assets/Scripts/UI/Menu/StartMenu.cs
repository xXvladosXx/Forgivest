using System;
using System.Text;
using System.Text.RegularExpressions;
using ModestTree;
using TMPro;
using UI.Menu.Core;
using UI.Warning;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class StartMenu : Core.Menu
    {
        [SerializeField] private string _sureWarningText;
        [SerializeField] private string _sameGameWarningText;
        [SerializeField] private string _emptyWarningText;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _startNewGame;
        [SerializeField] private TMP_InputField _inputField;
        
        private ChooseWarning _chooseWarning;
        private OneDecisionWarning _oneDecisionWarning;
        public event Action OnStartClicked;

        [Inject]
        public void Construct(ChooseWarning chooseWarning,
            OneDecisionWarning oneDecisionWarning)
        {
            _chooseWarning = chooseWarning;
            _oneDecisionWarning = oneDecisionWarning;
        }
        
        public void OnEnable()
        {
            _chooseWarning.enabled = false;
            _startNewGame.onClick.AddListener(TryToStartNewGame);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnDisable()
        {
            _startNewGame.onClick.RemoveListener(TryToStartNewGame);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
//            _inputField.text = string.Empty;
            SaveFile = string.Empty;
        }

        private void OnBackButtonClicked()
        {
            MenuSwitcher.Show<MainMenu>();
        }

        private void TryToStartNewGame()
        {
            var stringBuilder = new StringBuilder();
            if (Regex.IsMatch(stringBuilder.ToString(), @"^\s"))
            {
                stringBuilder.Append(_emptyWarningText);
                _oneDecisionWarning.ShowWarning(stringBuilder.ToString());
                
                return;    
            }
            
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

        private void StartNewGame()
        {
            //WarningUI.Instance.OnAccepted -= StartNewGame;
            //SaveInteractor.StartNewGame(SaveFile);
        }
    }
}