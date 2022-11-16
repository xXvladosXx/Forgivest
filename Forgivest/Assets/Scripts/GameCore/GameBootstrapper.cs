using GameCore.StateMachine.States;
using UI.Loading;
using UI.Menu;
using UI.Menu.Core;
using UnityEngine;

namespace GameCore
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [field: SerializeField] public LoadingScreen LoadingScreen { get; private set; }
        [field: SerializeField] public MenuSwitcher MainMenuSwitcher { get; private set; }
        [field: SerializeField] public Canvas MainMenuCanvas { get; private set; }
        
        private Game _game;

        private void Awake()
        {
            _game = new Game(this, LoadingScreen, MainMenuSwitcher, MainMenuCanvas);
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}