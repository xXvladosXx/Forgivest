﻿using System;
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
        
        public override void Initialize()
        {
            _saveNewGame.onClick.AddListener(TryToSaveNewGame);
        }
        
        private void TryToSaveNewGame()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(_warningText).Append("<i>").Append($" {SaveFile}").Append("</i>").Append("?");
            
            /*foreach (var saving in SaveInteractor.SaveList())
            {
                if (saving == SaveFile)
                {
                    stringBuilder.Clear();
                    stringBuilder.Append(_sameGameWarningText).Append("<i>").Append($" {SaveFile}").Append("</i>").Append("?");
                }
            }*/
        }
        
        public void CreateName(string saveFile)
        {
            SaveFile = saveFile;
        }
        
        private void SaveNewGame()
        {
            /*WarningUI.Instance.OnAccepted -= SaveNewGame;
            SaveInteractor.Save(SaveFile);*/
        }

        private void OnDisable()
        {
            SaveFile = "";
            _inputField.Select();
            _inputField.text = "";
        }
    }
}