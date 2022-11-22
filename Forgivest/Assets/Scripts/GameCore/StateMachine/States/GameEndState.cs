using UI.Utils;
using Zenject;

namespace GameCore.StateMachine.States
{
    public class GameEndState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ToFade _deathPanel;

        public GameEndState(GameStateMachine gameStateMachine, ToFade deathPanel)
        {
            _gameStateMachine = gameStateMachine;
            _deathPanel = deathPanel;
        }

        public void Enter()
        {
            _deathPanel.gameObject.SetActive(true);
            _deathPanel.TriggeredDeath();
        }

        public void Exit()
        {
            
        }
    }
}