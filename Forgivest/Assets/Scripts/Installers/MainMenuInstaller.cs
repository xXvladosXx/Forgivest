using Controllers;
using UI.Menu;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private StartMenu _startMenu;
        [SerializeField] private LoadMenu _loadMenu;
        [SerializeField] private SettingsMenu _settingsMenu;
        [SerializeField] private SoundMenu _soundMenu;
        [SerializeField] private GraphicsMenu _graphicsMenu;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuController>().AsSingle();
            Container.Bind<StartMenu>().FromInstance(_startMenu).AsSingle();
            Container.Bind<LoadMenu>().FromInstance(_loadMenu).AsSingle();
            Container.Bind<SettingsMenu>().FromInstance(_settingsMenu).AsSingle();
            Container.Bind<SoundMenu>().FromInstance(_soundMenu).AsSingle();
            Container.Bind<GraphicsMenu>().FromInstance(_graphicsMenu).AsSingle();
        }
    }
}