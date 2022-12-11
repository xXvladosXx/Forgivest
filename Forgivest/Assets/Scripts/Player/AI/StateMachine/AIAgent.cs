using System;
using System.Collections.Generic;
using AnimationSystem;
using AnimationSystem.Data;
using AttackSystem;
using AttackSystem.Core;
using AttackSystem.Reward.Core;
using Data.Player;
using InventorySystem.Core;
using InventorySystem.Interaction;
using InventorySystem.Items.Weapon;
using MovementSystem;
using Player.AI.StateMachine.Core;
using Player.AI.StateMachine.Core.Config;
using RaycastSystem.Core;
using Sirenix.OdinInspector;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using StatSystem.Scripts.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Player.AI.StateMachine
{
    public class AIAgent : SerializedMonoBehaviour, IAIEnemy, IAnimationEventUser, IDamageApplier, IDamageReceiver, IRaycastable, IInteractable
    {
        [field: SerializeField] public GameObject[] Objects { get; private set; }
        [field: SerializeField] public AliveEntityAnimationData AliveEntityAnimationData { get; private set; }
        [field: SerializeField] public AIAgentConfig Config { get; private set; }
        [field: SerializeField] public StatsFinder StatsFinder { get; private set; }
        [field: SerializeField] public CooldownSystem Cooldown { get; private set; }
        [field: SerializeField] public StatController StatController { get; private set; }

        [field: SerializeField] public List<IRewardable> Rewards { get; private set; }

        public Movement Movement { get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Transform Target { get; private set; }
        public AIStateMachine StateMachine { get; private set; }
        public AttackApplier AttackApplier { get; private set; }

        public Animator Animator { get; private set; }
        public AnimationChanger AnimationChanger { get; private set; }

        private Rigidbody _rb;
        private DamageHandler _damageHandler;

        public IAnimationEventUser AnimationEventUser => this;
        public GameObject Enemy => gameObject;

        public float Health => StatController.Health.CurrentValue;
        public LayerMask LayerMask => gameObject.layer;

        
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(IRaycastUser raycastUser)
        {
            return true;
        }

        public GameObject GameObject => gameObject == null ? null : gameObject;
        public event Action OnDestroyed;

        public void Interact()
        {
            
        }

        public void ReceiveDamage(AttackData attackData)
        {
            attackData.DamageReceiver = this;
            _damageHandler.TakeDamage(attackData);
            OnDamageReceived?.Invoke(attackData);
        }

        public event Action<AttackData> OnDamageReceived;


        private void Awake()
        {
            AliveEntityAnimationData.Init();
            _rb = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Movement = new Movement(NavMeshAgent, _rb, transform);
            AnimationChanger = new AnimationChanger(Animator);
            _damageHandler = new DamageHandler(StatController);

            AttackApplier = new AttackApplier();
        }

        private void Start()
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
            StateMachine = new AIStateMachine(this);
            StateMachine.ChangeState(StateMachine.AIIdleEnemyState);
        }

        private void OnEnable()
        {
            _damageHandler.OnDied += OnDied;
        }

        private void OnDied(AttackData attackData)
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            _damageHandler.OnDied -= OnDied;
        }
        
        private void Update()
        {
            StateMachine.Update();
        }

        public void OnAnimationStarted()
        {
            StateMachine.OnAnimationEnterEvent();
        }

        public void OnAnimationTransitioned()
        {
            StateMachine.OnAnimationTransitionEvent();
        }

        public void OnAnimationEnded()
        {
            StateMachine.OnAnimationExitEvent();
        }

        [field: SerializeField] public GameObject Weapon { get; private set; }
        [field: SerializeField] public Weapon CurrentWeapon { get; private set; }

        public void ApplyAttack(float timeOfActiveCollider)
        {
            var attackData = new AttackData
            {
                Damage = StatsFinder.FindStat("PhysicalAttack"),
                DamageApplierLayerMask = LayerMask,
                Weapon = CurrentWeapon,
                DamageApplier = this
            };

            AttackApplier.ApplyAttack(attackData, timeOfActiveCollider, Weapon);
        }

        public void ApplyShoot(Projectile projectile, Transform targetTransform, float definitionSpeed,
            ShotType definitionShotType,
            bool definitionIsSpin)
        {
        }

        public void TakeRewards(List<IRewardable> damageReceiverRewards)
        {
        }

        public event Action<AttackData> OnDamageApplied;

        public void CastedSkill()
        {
        }

        public void CastedProjectile()
        {
        }

        public void CastedSpawn()
        {
        }
    }
}