using AnimationSystem;
using AttackSystem;
using Data.Player;
using MovementSystem;
using RaycastSystem;
using RaycastSystem.Core;
using StateMachine.Player.States;
using StateMachine.Player.States.Attack;
using StateMachine.Player.States.Moving;
using Utilities;

namespace StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        public Rotator Rotator { get; }
        public AnimationChanger AnimationChanger { get; }
        public Movement Movement { get; }
        public PlayerInputProvider PlayerInputProvider { get; }
        public AliveEntityStateData AliveEntityStateData { get; }
        public PlayerRaycastUser RaycastUser { get; }
        public AliveEntityAnimationData AnimationData { get; }

        public AliveEntityStateReusableData ReusableData { get; }

        public PlayerIdlingState IdlingState { get; }
        public PlayerChasingState PlayerChasingState { get; }
        public PlayerCombatState PlayerCombatState { get; }
        public PlayerRunningState RunningState { get; }
        public AttackApplier AttackApplier { get; }

        public PlayerStateMachine(AnimationChanger animationChanger,
            Movement movement, Rotator rotator,
            PlayerInputProvider playerInputProvider,
            AliveEntityStateData aliveEntityStateData,
            PlayerRaycastUser raycastUser, 
            AliveEntityAnimationData animationData,
            AttackApplier attackApplier)
        {
            Rotator = rotator;
            AnimationChanger = animationChanger;
            Movement = movement;
            PlayerInputProvider = playerInputProvider;
            AliveEntityStateData = aliveEntityStateData;
            RaycastUser = raycastUser;
            AnimationData = animationData;
            AttackApplier = attackApplier;

            ReusableData = new AliveEntityStateReusableData();

            IdlingState = new PlayerIdlingState(this);
            RunningState = new PlayerRunningState(this);
            PlayerChasingState = new PlayerChasingState(this);
            PlayerCombatState = new PlayerCombatState(this);
        }
    }
}