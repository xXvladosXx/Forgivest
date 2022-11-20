using GameCore.StateMachine.States;
using UI.Loading;
using UI.Menu;
using UI.Menu.Core;
using UnityEngine;
using Zenject;

namespace GameCore
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [field: SerializeField] public LoadingScreen LoadingScreen { get; private set; }
        [field: SerializeField] public MenuSwitcher MainMenuSwitcher { get; private set; }
        [field: SerializeField] public Canvas MainMenuCanvas { get; private set; }

        private Game Game { get; set; }
        private DiContainer _container;

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }
        
        private void Awake()
        {
            Game = new Game(this, LoadingScreen, MainMenuSwitcher, _container);
            Game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
        
        
    }
}