using GameCore.StateMachine;
using UI.Loading;
using UI.Menu;
using UI.Menu.Core;
using UnityEngine;
using Utilities;
using Zenject;

namespace GameCore
{
    public class Game
    {
        public static PlayerInputProvider InputProvider;
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, 
            MenuSwitcher mainMenuSwitcher, DiContainer diContainer)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner),
                mainMenuSwitcher, diContainer);
        }
    }
}