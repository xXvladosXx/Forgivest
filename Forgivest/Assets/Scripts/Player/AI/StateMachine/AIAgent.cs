using System;
using System.Collections.Generic;
using AnimationSystem;
using AnimationSystem.Data;
using AttackSystem;
using AttackSystem.Core;
using AttackSystem.Reward.Core;
using Data.Player;
using InventorySystem.Interaction;
using InventorySystem.Items.Weapon;
using MovementSystem;
using Player.AI.StateMachine.Core;
using Player.AI.StateMachine.Core.Config;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using StatSystem.Scripts.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Player.AI.StateMachine
{
    public class AIAgent : MonoBehaviour, IAIEnemy, IAnimationEventUser, IDamageApplier, IDamageReceiver
    {
        [field: SerializeField] public GameObject[] Objects { get; private set; }
        [field: SerializeField] public AliveEntityAnimationData AliveEntityAnimationData { get; private set; }
        [field: SerializeField] public AIAgentConfig Config { get; private set; }
        [field: SerializeField] public StatsFinder StatsFinder { get; private set; }
        [field: SerializeField] public CooldownSystem Cooldown { get; private set; }
        [field: SerializeField] public StatController StatController { get; private set; }

        private Rigidbody _rb;

        public Movement Movement { get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Transform Target { get; private set; }
        public AIStateMachine StateMachine { get; private set; }
        public AttackApplier AttackApplier { get; private set; }

        public Animator Animator { get; private set; }
        public AnimationChanger AnimationChanger { get; private set; }

        private DamageHandler _damageHandler;

        public IAnimationEventUser AnimationEventUser => this;
        public GameObject Enemy => gameObject;

        public List<IRewardable> Rewards { get; }
        public float Health => StatController.Health.CurrentValue;
        public LayerMask LayerMask => gameObject.layer;

        public GameObject GameObject => gameObject == null ? null : gameObject;

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