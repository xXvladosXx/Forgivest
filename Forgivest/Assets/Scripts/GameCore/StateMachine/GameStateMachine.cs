using System;
using System.Collections.Generic;
using AttackSystem.Core;
using GameCore.Factory;
using GameCore.SaveSystem.Data;
using GameCore.SaveSystem.SaveLoad;
using GameCore.StateMachine.States;
using SoundSystem;
using UI.Loading;
using UI.Menu;
using UI.Menu.Core;
using UI.Utils;
using UnityEngine;
using Zenject;
using LoadMainMenuState = GameCore.StateMachine.States.LoadMainMenuState;

namespace GameCore.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _gameStates;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader,
            MenuSwitcher mainMenuSwitcher, DiContainer diContainer)
        {
            _gameStates = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, mainMenuSwitcher, diContainer.Resolve<SoundManger>()),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, diContainer.Resolve<PersistentCanvas>(), 
                    diContainer.Resolve<IGameFactory>(), diContainer.Resolve<IPersistentProgressService>(),
                    diContainer.Resolve<SoundManger>()),
                [typeof(LoadExistingGameState)] = new LoadExistingGameState(this, 
                    diContainer.Resolve<IPersistentProgressService>(),
                    diContainer.Resolve<ISaveLoadService>(), diContainer.Resolve<SoundManger>()),
                [typeof(GameLoopState)] = new GameLoopState(this, diContainer.Resolve<IGameFactory>(),
                    diContainer.Resolve<SoundManger>()),
                [typeof(GameEndState)] = new GameEndState(this, diContainer.Resolve<PersistentCanvas>()),
                [typeof(StartNewGameState)] = new StartNewGameState(this, diContainer.Resolve<IPersistentProgressService>(),
                    diContainer.Resolve<ISaveLoadService>(), diContainer.Resolve<SoundManger>()),
                [typeof(LoadMainMenuState)] = new LoadMainMenuState(this, diContainer.Resolve<IPersistentProgressService>(),
                    diContainer.Resolve<ISaveLoadService>(), diContainer.Resolve<SoundManger>())
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