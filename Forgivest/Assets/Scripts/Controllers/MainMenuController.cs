using System;
using GameCore.SaveSystem.Reader;
using GameCore.SaveSystem.Scripts.Runtime;
using UI.Menu;
using Zenject;

namespace Controllers
{
    public class MainMenuController : IInitializable, IDisposable
    {
        private readonly StartMenu _startMenu;
        private readonly LoadMenu _loadMenu;
        private readonly SettingsMenu _settingsMenu;

        public MainMenuController(StartMenu startMenu,
            LoadMenu loadMenu, SettingsMenu settingsMenu)
        {
            _startMenu = startMenu;
            _loadMenu = loadMenu;
            _settingsMenu = settingsMenu;
        }

        public void Initialize()
        {
            _loadMenu.Initialize(FileManager.SavesList());
        }

        public void Dispose()
        {
        }
    }
}