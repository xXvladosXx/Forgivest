using System;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.AbilitySystem.Runtime;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active;
using AbilitySystem.AbilitySystem.Runtime.Effects;
using AnimationSystem;
using AnimationSystem.Data;
using AttackSystem;
using AttackSystem.ColliderSystem;
using AttackSystem.Core;
using AttackSystem.Reward;
using AttackSystem.Reward.Core;
using Data.Player;
using GameCore.Factory;
using GameCore.StateMachine;
using InventorySystem.Interaction;
using InventorySystem.Items;
using InventorySystem.Items.Weapon;
using InventorySystem.Requirements.Core;
using LevelSystem;
using LevelSystem.Scripts.Runtime;
using MovementSystem;
using Player.StateMachine.Player;
using RaycastSystem;
using RaycastSystem.Core;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using StatSystem.Scripts.Runtime;
using UnityEngine;
using UnityEngine.AI;
using Utilities;
using Zenject;


namespace Player
{
    [RequireComponent(typeof(NavMeshAgent),
        typeof(Rigidbody),
        typeof(Collider))]
    
    [RequireComponent(typeof(StatController),
        typeof(Animator),
        typeof(ObjectPicker))]
    
    [RequireComponent(typeof(AbilityHandler),
        typeof(GameplayEffectHandler))]
    public class PlayerEntity : MonoBehaviour, IAnimationEventUser, IDamageReceiver,
        IDamageApplier, IRaycastable
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
        [field: SerializeField] public AbilityHandler AbilityHandler { get; private set; }
        [field: SerializeField] public GameplayEffectHandler GameplayEffectHandler { get; private set; }
        [field: SerializeField] public PlayerRaycastSettings PlayerRaycastSettings { get; private set; }
        [field: SerializeField] public ObjectPicker ObjectPicker { get; private set; }

        [field: Header("Level System")]
        [field: SerializeField] public LevelController LevelController { get; private set; }
        [SerializeField] private int _pointsPerLevel;
        public Camera Camera { get; private set; }
        public AttackApplier AttackApplier { get; private set; }
        public AnimationChanger AnimationChanger { get; private set; }
        public Movement Movement { get; private set; }
        public Rotator Rotator { get; private set; }
        public PlayerRaycastUser RaycastUser { get; private set; }
        public DamageHandler DamageHandler { get; private set; }
        public ItemRequirementsChecker ItemRequirementsChecker { get; private set; }
        public AbilitiesRequirementsChecker AbilitiesRequirementsChecker { get; private set; }

        private PlayerStateMachine _playerStateMachine;
        
        public GameObject Weapon => ObjectPicker.ItemEquipHandler.CurrentColliderWeapon;
        public Weapon CurrentWeapon => ObjectPicker.ItemEquipHandler.CurrentWeapon;
        public List<IRewardable> Rewards { get; }
        public float Health => StatsFinder.FindStat("Health");
        public LayerMask LayerMask => gameObject.layer;
        public GameObject GameObject => gameObject;

        public event Action<AttackData> OnDamageReceived;
        public event Action<AttackData> OnDamageApplied;
        
        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            AnimationChanger = new AnimationChanger(Animator);
            Movement = new Movement(NavMeshAgent, Rigidbody, transform);
            Rotator = new Rotator(Rigidbody);
            AttackApplier = new AttackApplier();
            DamageHandler = new DamageHandler(StatController);
            ItemRequirementsChecker = new ItemRequirementsChecker(ObjectPicker.Inventory.ItemContainer,
                ObjectPicker.Equipment.ItemContainer, LevelController);

            AbilitiesRequirementsChecker = new AbilitiesRequirementsChecker(AbilityHandler.LearnedAbilities.ItemContainer, 
                LevelController, AbilityHandler);
            
            gameFactory.PlayerObserver.DamageHandler ??= DamageHandler;
            gameFactory.PlayerObserver.PlayerInputProvider ??= PlayerInputProvider;
            
            gameFactory.Register(Movement);
        }

        private void Awake()
        {
            Camera = Camera.main;
            AliveEntityAnimationData.Init();
            
            RaycastUser = new PlayerRaycastUser(Camera, PlayerInputProvider, PlayerRaycastSettings);
            
            _playerStateMachine = new PlayerStateMachine(AnimationChanger,
                Movement, Rotator, PlayerInputProvider, StateData,
                RaycastUser, AliveEntityAnimationData, 
                this, AbilityHandler, DamageHandler, ItemRequirementsChecker);
        }
        
        private void OnEnable()
        {
            ObjectPicker.ItemEquipHandler.OnWeaponEquipped += OnWeaponEquipped;
            LevelController.OnLevelChanged += OnLevelChanged;
        }

        private void Start()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.IdlingState);
            ObjectPicker.Init(ItemRequirementsChecker);
        }

        private void Update()
        {
            RaycastUser.Tick();
            _playerStateMachine.Update();
        }

        private void OnDisable()
        {
            ObjectPicker.ItemEquipHandler.OnWeaponEquipped -= OnWeaponEquipped;
            LevelController.OnLevelChanged -= OnLevelChanged;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Movement;
        }

        public bool HandleRaycast(IRaycastUser raycastUser)
        {
            return true;
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
            if (AbilityHandler.CurrentAbility is SingleTargetAbility singeTargetAbility)
            {
                var raycastable = _playerStateMachine.ReusableData.SkillRaycastable;
                
                switch (raycastable)
                {
                    case IDamageReceiver damageReceiver:
                        var attackData = new AttackData
                        {
                            DamageApplierLayerMask = LayerMask,
                            DamageApplier = this,
                            DamageReceiver = damageReceiver,
                        };

                        singeTargetAbility.Cast(singeTargetAbility.ActiveAbilityDefinition.SelfCasted
                            ? gameObject
                            : _playerStateMachine.ReusableData.SkillRaycastable.GameObject, attackData);
                        break;
                    case null:
                        if(singeTargetAbility.ActiveAbilityDefinition.SelfCasted)
                            singeTargetAbility.Cast(gameObject, null);
                        break;
                }
            }
        }

        public void CastedProjectile()
        {
            if (AbilityHandler.CurrentAbility is ProjectileAbility projectileAbility)
            {
                var raycastable = _playerStateMachine.ReusableData.SkillRaycastable;

                switch (raycastable)
                {
                    case IDamageReceiver damageReceiver:
                        var attackData = new AttackData
                        {
                            DamageApplierLayerMask = LayerMask,
                            DamageApplier = this,
                            DamageReceiver = damageReceiver,
                        };
                        
                        projectileAbility.Shoot(attackData);
                        break;
                }
            }
        }

        public void CastedSpawn()
        {
            if (AbilityHandler.CurrentAbility is RadiusDamageAbility radiusDamageAbility)
            {
                var attackData = new AttackData
                {
                    DamageApplier = this,
                    DamageApplierLayerMask = gameObject.layer,
                    Source = gameObject
                };
                
                radiusDamageAbility.Spawn(_playerStateMachine.ReusableData.HoveredPoint, attackData);
            }
        }

        private void OnWeaponEquipped(Weapon weapon)
        {
            AnimationChanger.ChangeRuntimeAnimatorController(weapon.AnimatorController);
        }

        public void ReceiveDamage(AttackData attackData)
        {
            Debug.Log("Player recieved damage");
            attackData.DamageReceiver = this;
            DamageHandler.TakeDamage(attackData);
            OnDamageReceived?.Invoke(attackData);
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
                        LevelController.CurrentExperience += experienceReward.Amount;
                        break;
                    case ItemReward itemReward:
                        ObjectPicker.TryToEquipOrAddToInventory(itemReward.Item, itemReward.Amount);
                        break;
                }
            }
        }
        
        private void OnLevelChanged()
        {
            AbilityHandler.AddPoints(_pointsPerLevel);   
        }
    }
}
