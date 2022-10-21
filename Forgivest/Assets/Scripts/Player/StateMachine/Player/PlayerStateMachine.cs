using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AnimationSystem;
using AnimatorStateMachine.StateMachine;
using AttackSystem;
using AttackSystem.Core;
using Data.Player;
using MovementSystem;
using Player.States.Moving;
using Player.States.Skill.Skills;
using RaycastSystem;
using RaycastSystem.Core;
using StateMachine.Player.States;
using StateMachine.Player.States.Attack;
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
        public IDamageApplier AttackApplier { get; }
        public AbilityController AbilityController { get; }
        public PlayerFirstCastState PlayerFirstCastState { get; }
        public PlayerSecondCastState PlayerSecondCastState { get; }
        public PlayerThirdCastState PlayerThirdCastState { get; }
        public PlayerFourthCastState PlayerFourthCastState { get; }
        public PlayerFifthCastState PlayerFifthCastState { get; }

        public PlayerStateMachine(AnimationChanger animationChanger,
            Movement movement, Rotator rotator,
            PlayerInputProvider playerInputProvider,
            AliveEntityStateData aliveEntityStateData,
            PlayerRaycastUser raycastUser,
            AliveEntityAnimationData animationData,
            IDamageApplier attackApplier, AbilityController abilityController)
        {
            Rotator = rotator;
            AnimationChanger = animationChanger;
            Movement = movement;
            PlayerInputProvider = playerInputProvider;
            AliveEntityStateData = aliveEntityStateData;
            RaycastUser = raycastUser;
            AnimationData = animationData;
            AttackApplier = attackApplier;
            AbilityController = abilityController;

            ReusableData = new AliveEntityStateReusableData();

            IdlingState = new PlayerIdlingState(this);
            RunningState = new PlayerRunningState(this);
            PlayerChasingState = new PlayerChasingState(this);
            PlayerCombatState = new PlayerCombatState(this);
            
            PlayerFirstCastState = new PlayerFirstCastState(this);
            PlayerSecondCastState = new PlayerSecondCastState(this);
            PlayerThirdCastState = new PlayerThirdCastState(this);
            PlayerFourthCastState = new PlayerFourthCastState(this);
            PlayerFifthCastState = new PlayerFifthCastState(this);
        }
    }
}