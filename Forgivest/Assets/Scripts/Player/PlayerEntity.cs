using System;
using Data.Player;
using StateMachine.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utilities;
using Utilities.Animation;

namespace Player
{
    public class PlayerEntity : MonoBehaviour, IAnimationEventUser
    {
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public PlayerInputProvider PlayerInputProvider { get; private set; }
        [field: SerializeField] public AliveEntityAnimationData AnimationData { get; private set; }
        [field: SerializeField] public AliveEntityStateData StateData { get; set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        public Camera Camera { get; private set; }

        private PlayerStateMachine _playerStateMachine;
        private void Awake()
        {
            Camera = Camera.main;
            AnimationData.Init();

            _playerStateMachine = new PlayerStateMachine(this);
        }

        private void Start()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.IdlingState);
        }

        private void Update()
        {
            _playerStateMachine.Update();
            _playerStateMachine.HandleInput();
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
