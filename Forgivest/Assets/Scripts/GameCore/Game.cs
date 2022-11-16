using GameCore.StateMachine;
using UI.Loading;
using UI.Menu;
using UI.Menu.Core;
using UnityEngine;
using Utilities;

namespace GameCore
{
    public class Game
    {
        public static PlayerInputProvider InputProvider;
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingScreen loadingScreen, 
            MenuSwitcher mainMenuSwitcher, Canvas mainMenuCanvas)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingScreen, mainMenuSwitcher, mainMenuCanvas);
        }
    }
}