using System;
using System.Collections.Generic;
using GameCore.Data;
using GameCore.Data.SaveLoad;
using GameCore.Factory;
using GameCore.StateMachine.States;
using UI.Loading;
using UI.Menu;
using UI.Menu.Core;
using UnityEngine;
using Zenject;

namespace GameCore.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _gameStates;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingScreen loadingScreen,
            MenuSwitcher mainMenuSwitcher, DiContainer diContainer)
        {
            _gameStates = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, mainMenuSwitcher, diContainer),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingScreen, 
                    diContainer.Resolve<IGameFactory>(),
                    diContainer.Resolve<IPersistentProgressService>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this, 
                    diContainer.Resolve<IPersistentProgressService>(),
                    diContainer.Resolve<ISaveLoadService>()),
                [typeof(GameLoopState)] = new GameLoopState(this, diContainer),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
             TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TLoad>(TLoad load) where TState : class, ILoadState<TLoad>
        {
            TState state = ChangeState<TState>();
            state.Enter(load);
        }
        
        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }
        
        private TState GetState<TState>() where TState : class, IExitableState 
            => _gameStates[typeof(TState)] as TState;
    }
}