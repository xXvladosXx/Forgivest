using AbilitySystem.AbilitySystem.Runtime.Abilities;
using CombatSystem.Scripts.Runtime;
using LevelSystem;
using StatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace MyGame.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AbilityController))]
    public class Player : CombatableCharacter
    {
        private ILevelable m_Levelable;
        [SerializeField] private Transform m_Target;
        private NavMeshAgent m_NavMeshAgent;
        private AbilityController _abilityController;


        protected override void Awake()
        {
            base.Awake();
            m_Levelable = GetComponent<ILevelable>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            _abilityController = GetComponent<AbilityController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var actived = _abilityController.TryActiveAbility("Shock", m_Target.gameObject);
                print(actived);
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                var actived = _abilityController.TryActiveAbility("Heal", m_Target.gameObject);
                print(actived);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                var actived = _abilityController.TryActiveAbility("Regeneration", m_Target.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                var actived = _abilityController.TryActiveAbility("Poison", m_Target.gameObject);
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                var actived = _abilityController.TryActiveAbility("Malediction", m_Target.gameObject);
            }
        }
    }
}