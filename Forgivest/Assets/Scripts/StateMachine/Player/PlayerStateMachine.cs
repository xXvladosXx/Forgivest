using AnimationSystem;
using AnimatorStateMachine.StateMachine;
using Data.Player;
using MovementSystem;
using RaycastSystem.Core;
using StateMachine.Player.Movement;
using StateMachine.Player.StateMachines.Movement.States.Grounded;
using StateMachine.Player.StateMachines.Movement.States.Grounded.Attack;
using StateMachine.Player.StateMachines.Movement.States.Grounded.Moving;
using Utilities;

namespace StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        public Rotator Rotator { get; }
        public AnimationChanger AnimationChanger { get; }
        public MovementSystem.Movement Movement { get; }
        public PlayerInputProvider PlayerInputProvider { get; }
        public AliveEntityStateData AliveEntityStateData { get; }
        public RaycastUser RaycastUser { get; }
        public AliveEntityAnimationData AnimationData { get; }

        public AliveEntityStateReusableData ReusableData { get; }

        public PlayerIdlingState IdlingState { get; }
        public PlayerAggroState PlayerAggroState { get; }
        public PlayerCombatState PlayerCombatState { get; }

        public PlayerRunningState RunningState { get; }

        public PlayerStateMachine(AnimationChanger animationChanger,
            MovementSystem.Movement movement, Rotator rotator,
            PlayerInputProvider playerInputProvider,
            AliveEntityStateData aliveEntityStateData,
            RaycastUser raycastUser, 
            AliveEntityAnimationData animationData)
        {
            Rotator = rotator;
            AnimationChanger = animationChanger;
            Movement = movement;
            PlayerInputProvider = playerInputProvider;
            AliveEntityStateData = aliveEntityStateData;
            RaycastUser = raycastUser;
            AnimationData = animationData;

            ReusableData = new AliveEntityStateReusableData();

            IdlingState = new PlayerIdlingState(this);

            RunningState = new PlayerRunningState(this);
            PlayerAggroState = new PlayerAggroState(this);
            PlayerCombatState = new PlayerCombatState(this);
        }
    }
}