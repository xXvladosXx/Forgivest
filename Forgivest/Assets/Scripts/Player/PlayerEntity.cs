using System;
using Data.Player;
using StateMachine.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class PlayerEntity : MonoBehaviour
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
        
    }
}
