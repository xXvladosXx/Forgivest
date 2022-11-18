using UnityEngine;

namespace UI.InputChecker
{
    public class MainMenuInputChecker : InputChecker
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _gameHeader;

        protected virtual void ShowMenu()
        {
            _menu.SetActive(true);
        }

        public override void Hide()
        {
            base.Hide();
            
            _gameHeader.SetActive(false);
            ShowMenu();
        }
    }
}