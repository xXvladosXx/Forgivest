using AbilitySystem.AbilitySystem.Runtime.Abilities;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using StatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace MyGame
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(StatController))]
    [RequireComponent(typeof(AbilityHandler))]
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private float m_BaseSpeed = 3.5f;
        private Animator m_Animator;
        private NavMeshAgent m_NavMeshAgent;
        private StatController m_StatController;
        private AbilityHandler _abilityHandler;
        
        private static readonly int MOVEMENT_SPEED = Animator.StringToHash("MovementSpeed");
        private static readonly int VELOCITY = Animator.StringToHash("Velocity");
        private static readonly int ATTACK_SPEED = Animator.StringToHash("AttackSpeed");

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_StatController = GetComponent<StatController>();
            _abilityHandler = GetComponent<AbilityHandler>();
        }

        private void Update()
        {
            m_Animator.SetFloat(VELOCITY, m_NavMeshAgent.velocity.magnitude / m_NavMeshAgent.speed);
        }

        private void OnEnable()
        {
            m_StatController.OnInitialized += OnStatControllerOnInitialized;
            if (m_StatController.IsInitialized)
            {
                OnStatControllerOnInitialized();
            }
            
            _abilityHandler.OnAbilityActivated += ActivateAbility;
        }

        private void OnDisable()
        {
            m_StatController.OnInitialized -= OnStatControllerOnInitialized;
            if (m_StatController.IsInitialized)
            {
                //m_StatController.Stats["MovementSpeed"].OnValueChanged -= OnMovementSpeedChanged;
                //m_StatController.Stats["AttackSpeed"].OnValueChanged -= OnAttackSpeedChanged;
            }

            _abilityHandler.OnAbilityActivated -= ActivateAbility;
        }

        public void Cast()
        {
            if (_abilityHandler.CurrentAbility is SingleTargetAbility singeTargetAbility)
            {
                //singeTargetAbility.Cast(_abilityController.Target);
            }
        }

        public void Shoot()
        {
            if (_abilityHandler.CurrentAbility is ProjectileAbility projectileAbility)
            {
            }
        }

        private void ActivateAbility(ActiveAbility activeAbility)
        {
            m_Animator.SetTrigger(activeAbility.ActiveAbilityDefinition.AnimationName);
        }

        private void OnStatControllerOnInitialized()
        {
            OnMovementSpeedChanged();
            OnAttackSpeedChanged();
            //m_StatController.Stats["MovementSpeed"].OnValueChanged += OnMovementSpeedChanged;
            //m_StatController.Stats["AttackSpeed"].OnValueChanged += OnAttackSpeedChanged;
        }
        
        private void OnAttackSpeedChanged()
        {
            m_Animator.SetFloat(ATTACK_SPEED, m_StatController.Stats["AttackSpeed"].Value / 100f);
        }

        private void OnMovementSpeedChanged()
        {
            m_Animator.SetFloat(MOVEMENT_SPEED, m_StatController.Stats["MovementSpeed"].Value / 100f);
            m_NavMeshAgent.speed = m_BaseSpeed * m_StatController.Stats["MovementSpeed"].Value / 100f;
        }
    }
}