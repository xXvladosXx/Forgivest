using System.Text;
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
        public override void Initialize()
        {
            
            OnEnable();
        }
        
        private void OnEnable()
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
            
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