using System;
using AnimationSystem;
using AttackSystem;
using AttackSystem.Core;
using Data.Player;
using InventorySystem.Interaction;
using MovementSystem;
using RaycastSystem;
using RaycastSystem.Core;
using StateMachine.Player;
using StatsSystem;
using StatsSystem.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    [RequireComponent(typeof(NavMeshAgent),
        typeof(Rigidbody),
        typeof(Collider))]
    
    [RequireComponent(typeof(StatsHandler),
        typeof(Animator),
        typeof(ObjectPicker))]
    public class PlayerEntity : MonoBehaviour, IAnimationEventUser, IDamageReceiver
    {
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public PlayerInputProvider PlayerInputProvider { get; private set; }
        [field: SerializeField] public AliveEntityAnimationData AliveEntityAnimationData { get; private set; }
        [field: SerializeField] public AliveEntityStateData StateData { get; set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public StatsHandler StatsHandler { get; private set; }

        [SerializeField] private ObjectPicker _objectPicker;

        public Camera Camera { get; private set; }
        
        public AnimationChanger AnimationChanger { get; private set; }
        public Movement Movement { get; private set; }
        public Rotator Rotator { get; private set; }
        public RaycastUser RaycastUser { get; private set; }
        
        public AttackApplier AttackApplier { get; private set; }
        
        private PlayerStateMachine _playerStateMachine;
        
        public event Action<AttackData> OnDamageReceived;
        public LayerMask LayerMask => gameObject.layer;

        public void ReceiverDamage(AttackData attackData)
        {
            Debug.Log("Player recieved damage");
        }
        
        private void Awake()
        {
            Camera = Camera.main;
            AliveEntityAnimationData.Init();

            AnimationChanger = new AnimationChanger(Animator);
            Movement = new Movement(NavMeshAgent, Rigidbody, transform);
            Rotator = new Rotator(Rigidbody);
            RaycastUser = new PlayerRaycastUser(Camera);

            _objectPicker.Init(RaycastUser);
            
            AttackApplier = new AttackApplier(_objectPicker.ItemEquipHandler);
            
            _playerStateMachine = new PlayerStateMachine(AnimationChanger,
                Movement, Rotator, PlayerInputProvider, StateData,
                RaycastUser, AliveEntityAnimationData, 
                AttackApplier);
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

        public void ApplyAttack(float timeOfActiveCollider)
        {
            AttackApplier.ApplyDamage(new AttackData
            {
                Damage = StatsHandler.CalculateStat(StatsEnum.Damage),
                DamageApplierLayerMask = LayerMask
            }, timeOfActiveCollider);
        }

        
    }
}
