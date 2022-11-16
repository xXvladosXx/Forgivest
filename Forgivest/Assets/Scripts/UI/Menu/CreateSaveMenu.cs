using TMPro;
using UI.Menu.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class CreateSaveMenu : Core.Menu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _startNewGame;
        [SerializeField] private TextMeshProUGUI _warning;
        
        public override void Initialize()
        {
            _startNewGame.onClick.AddListener(SaveNewGame);
        }
        
        public void CreateName(string saveFile)
        {
            SaveFile = saveFile;
        }

        private void SaveNewGame()
        {
            if (SaveFile.Length > 0)
            {
                _warning.text = "";
                /*SaveInteractor.Save(SaveFile);*/
            }
            else
            {
                _warning.text = "Fill the name";
            }
        }
    }
}