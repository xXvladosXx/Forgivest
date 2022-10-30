using System;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using AnimationSystem;
using AttackSystem;
using AttackSystem.Core;
using AttackSystem.Reward;
using AttackSystem.Reward.Core;
using Data.Player;
using InventorySystem.Interaction;
using InventorySystem.Items.Weapon;
using LevelSystem;
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
    
    [RequireComponent(typeof(AbilityController),
        typeof(GameplayEffectHandler))]
    public class PlayerEntity : MonoBehaviour, IAnimationEventUser, IDamageReceiver, IDamageApplier
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
        [field: SerializeField] public AbilityController AbilityController { get; private set; }
        [field: SerializeField] public GameplayEffectHandler GameplayEffectHandler { get; private set; }
        [field: SerializeField] public GameObject Target { get; private set; }
        [field: SerializeField] public PlayerRaycastSettings PlayerRaycastSettings { get; private set; }
        
        [SerializeField] private ObjectPicker _objectPicker;
        [SerializeField] private LevelController _levelController;
        public Camera Camera { get; private set; }
        public AttackApplier AttackApplier { get; private set; }
        public AnimationChanger AnimationChanger { get; private set; }
        public Movement Movement { get; private set; }
        public Rotator Rotator { get; private set; }
        public PlayerRaycastUser RaycastUser { get; private set; }
        
        private PlayerStateMachine _playerStateMachine;
        private DamageHandler _damageHandler;

        public GameObject Weapon => _objectPicker.ItemEquipHandler.CurrentColliderWeapon;
        public Weapon CurrentWeapon => _objectPicker.ItemEquipHandler.CurrentWeapon;
        public List<IRewardable> Rewards { get; }
        public LayerMask LayerMask => gameObject.layer;
        public GameObject GameObject => gameObject;

        public event Action<AttackData> OnDamageReceived;
        

        public event Action<AttackData> OnDamageApplied;

        private void Awake()
        {
            Camera = Camera.main;
            AliveEntityAnimationData.Init();

            AnimationChanger = new AnimationChanger(Animator);
            Movement = new Movement(NavMeshAgent, Rigidbody, transform);
            Rotator = new Rotator(Rigidbody);
            RaycastUser = new PlayerRaycastUser(Camera, PlayerInputProvider, PlayerRaycastSettings);
            AttackApplier = new AttackApplier();
            _damageHandler = new DamageHandler(StatController);
            
            _playerStateMachine = new PlayerStateMachine(AnimationChanger,
                Movement, Rotator, PlayerInputProvider, StateData,
                RaycastUser, AliveEntityAnimationData, 
                this, AbilityController);
        }
        
        private void OnEnable()
        {
            _objectPicker.ItemEquipHandler.OnWeaponEquipped += OnWeaponEquipped;
        }

        private void Start()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.IdlingState);
            _objectPicker.Init();
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

        public void CastedSkill()
        {
            if (AbilityController.CurrentAbility is SingleTargetAbility singeTargetAbility)
            {
                singeTargetAbility.Cast(AbilityController.Target);
            }
        }

        public void CastedProjectile()
        {
            if (AbilityController.CurrentAbility is ProjectileAbility projectileAbility)
            {
                var raycastable = _playerStateMachine.ReusableData.Raycastable;
                switch (raycastable)
                {
                    case null:
                        return;
                    case IDamageReceiver damageReceiver:
                        projectileAbility.Shoot(_playerStateMachine.ReusableData.Raycastable.GameObject, this);
                        break;
                }
            }
        }

        private void OnWeaponEquipped(Weapon weapon)
        {
            AnimationChanger.ChangeRuntimeAnimatorController(weapon.AnimatorController);
        }

        public void ReceiveDamage(AttackData attackData)
        {
            Debug.Log("Player recieved damage");
            _damageHandler.TakeDamage(attackData);
        }
        
        public void GetReward(object reward)
        {
            if (reward is ExperienceReward experienceReward)
            {
                _levelController.CurrentExperience += experienceReward.Amount;
            }
        }
        
        public void ApplyShoot(Projectile projectile, Transform targetTransform,
            float definitionSpeed, ShotType definitionShotType,
            bool definitionIsSpin)
        {
            if (Weapon.TryGetComponent(out RangedWeapon rangedWeapon))
            {
                rangedWeapon.Shoot(projectile, targetTransform, definitionSpeed,
                    gameObject.layer, definitionShotType, definitionIsSpin);
                
            }
        }

       

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
        
        public void TakeRewards(List<IRewardable> damageReceiverRewards)
        {
            foreach (var rewardable in damageReceiverRewards)
            {
                switch (rewardable)
                {
                    case ExperienceReward experienceReward:
                        _levelController.CurrentExperience += experienceReward.Amount;
                        break;
                    case ItemReward itemReward:
                        _objectPicker.TryToEquipOrAddToInventory(itemReward.Item, itemReward.Amount);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rewardable));
                }
            }
        }
    }
}
