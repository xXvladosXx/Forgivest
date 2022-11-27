using System;
using System.Text;
using TMPro;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class CreateGameMenu : Core.Menu
    {
        [SerializeField] private string _warningText;
        [SerializeField] private string _sameGameWarningText;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _saveNewGame;
        [SerializeField] private TMP_InputField _inputField;
        
        private void TryToSaveNewGame()
        {
            var stringBuilder = new StringBuilder();
            
            /*foreach (var saving in SaveInteractor.SaveList())
            {
                if (saving == SaveFile)
                {
                    stringBuilder.Clear();
                    stringBuilder.Append(_sameGameWarningText).Append("<i>").Append($" {SaveFile}").Append("</i>").Append("?");
                }
            }*/
        }
        
        private void SaveNewGame()
        {
            /*WarningUI.Instance.OnAccepted -= SaveNewGame;
            SaveInteractor.Save(SaveFile);*/
        }

       
    }
}