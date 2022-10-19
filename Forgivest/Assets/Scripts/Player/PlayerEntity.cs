using System;
using AnimationSystem;
using AttackSystem;
using AttackSystem.Core;
using Data.Player;
using InventorySystem.Interaction;
using InventorySystem.Items.Weapon;
using MovementSystem;
using RaycastSystem;
using RaycastSystem.Core;
using StateMachine.Player;
using StatSystem;
using StatSystem.Scripts.Runtime;
using UnityEngine;
using UnityEngine.AI;
using Utilities;


namespace Player
{
    [RequireComponent(typeof(NavMeshAgent),
        typeof(Rigidbody),
        typeof(Collider))]
    
    [RequireComponent(typeof(StatController),
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
        [field: SerializeField] public StatController StatController { get; private set; }
        [field: SerializeField] public StatsFinder StatsFinder { get; private set; }

        [field: SerializeField] public PlayerRaycastSettings PlayerRaycastSettings { get; private set; }
        [SerializeField] private ObjectPicker _objectPicker;

        public Camera Camera { get; private set; }
        
        public AnimationChanger AnimationChanger { get; private set; }
        public Movement Movement { get; private set; }
        public Rotator Rotator { get; private set; }
        public PlayerRaycastUser RaycastUser { get; private set; }
        
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
            RaycastUser = new PlayerRaycastUser(Camera, PlayerInputProvider, PlayerRaycastSettings, Movement);
            
            AttackApplier = new AttackApplier(_objectPicker.ItemEquipHandler);
            
            _playerStateMachine = new PlayerStateMachine(AnimationChanger,
                Movement, Rotator, PlayerInputProvider, StateData,
                RaycastUser, AliveEntityAnimationData, 
                AttackApplier);
        }
        
        private void OnEnable()
        {
            _objectPicker.ItemEquipHandler.OnWeaponEquipped += OnWeaponEquipped;
        }
        
        private void Start()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.IdlingState);
            _objectPicker.Init(RaycastUser);
        }

        private void Update()
        {
            RaycastUser.Tick();
            _playerStateMachine.Update();
        }

        private void OnDisable()
        {
            _objectPicker.ItemEquipHandler.OnWeaponEquipped -= OnWeaponEquipped;
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
            Debug.Log(StatsFinder.FindStat("PhysicalAttack"));
            
            /*Debug.Log(StatsHandler.CalculateStat(StatsEnum.Health) + " Health ");
            Debug.Log(StatsHandler.CalculateStat(StatsEnum.Damage) + " Dam ");
            */
            
            AttackApplier.ApplyDamage(new AttackData
            {
                Damage = StatsFinder.FindStat("PhysicalAttack"),
                DamageApplierLayerMask = LayerMask
            }, timeOfActiveCollider);
            
        }

        private void OnWeaponEquipped(Weapon weapon)
        {
            AnimationChanger.ChangeRuntimeAnimatorController(weapon.AnimatorController);
        }
    }
}
