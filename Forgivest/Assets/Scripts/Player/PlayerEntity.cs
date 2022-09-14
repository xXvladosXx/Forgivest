using System;
using AnimationSystem;
using Data.Player;
using InventorySystem.Interaction;
using MovementSystem;
using RaycastSystem;
using RaycastSystem.Core;
using StateMachine.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class PlayerEntity : MonoBehaviour, IAnimationEventUser
    {
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public PlayerInputProvider PlayerInputProvider { get; private set; }
        [field: SerializeField] public AliveEntityAnimationData AliveEntityAnimationData { get; private set; }
        [field: SerializeField] public AliveEntityStateData StateData { get; set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        [SerializeField] private ObjectPicker _objectPicker;

        public Camera Camera { get; private set; }
        
        public AnimationChanger AnimationChanger { get; private set; }
        public Movement Movement { get; private set; }
        public Rotator Rotator { get; private set; }
        public RaycastUser RaycastUser { get; private set; }
        
        private PlayerStateMachine _playerStateMachine;
        private void Awake()
        {
            Camera = Camera.main;
            AliveEntityAnimationData.Init();

            AnimationChanger = new AnimationChanger(Animator);
            Movement = new Movement(NavMeshAgent, Rigidbody, transform);
            Rotator = new Rotator(Rigidbody);
            RaycastUser = new PlayerRaycastUser(Camera);

            _objectPicker.Init(RaycastUser);
            
            _playerStateMachine = new PlayerStateMachine(AnimationChanger,
                Movement, Rotator, PlayerInputProvider, StateData,
                RaycastUser, AliveEntityAnimationData);
        }

        private void Start()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.IdlingState);
        }

        private void Update()
        {
            _playerStateMachine.Update();
        }

        public void OnAnimationStarted()
        {
            _playerStateMachine.OnAnimationEnterEvent();
        }

        public void OnAnimationTransitioned()
        {
            _playerStateMachine.OnAnimationTransitionEvent();
        }

        public void OnAnimationEnded()
        {
            _playerStateMachine.OnAnimationExitEvent();
        }
    }
}
