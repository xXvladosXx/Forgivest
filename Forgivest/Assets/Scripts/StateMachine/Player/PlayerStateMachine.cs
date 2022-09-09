using Data.Player;
using Player;
using StateMachine.Player.StateMachines.Movement.States.Grounded;
using StateMachine.Player.StateMachines.Movement.States.Grounded.Attack;
using StateMachine.Player.StateMachines.Movement.States.Grounded.Moving;

namespace StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        public PlayerEntity Player { get; }
        public AliveEntityStateReusableData ReusableData { get; }

        public PlayerIdlingState IdlingState { get; }
        public PlayerDashingState DashingState { get; }
        public PlayerAggroState PlayerAggroState { get; }
        public PlayerCombatState PlayerCombatState { get; }

        public PlayerRunningState RunningState { get; }
        

        public PlayerStateMachine(PlayerEntity player)
        {
            Player = player;
            ReusableData = new AliveEntityStateReusableData();

            IdlingState = new PlayerIdlingState(this);
            DashingState = new PlayerDashingState(this);

            RunningState = new PlayerRunningState(this);
            PlayerAggroState = new PlayerAggroState(this);
            PlayerCombatState = new PlayerCombatState(this);
        }
        
    }
}