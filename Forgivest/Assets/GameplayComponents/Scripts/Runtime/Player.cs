using AbilitySystem.AbilitySystem.Runtime.Abilities;
using CombatSystem.Scripts.Runtime;
using LevelSystem;
using LevelSystem.Scripts.Runtime;
using StatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace MyGame.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AbilityHandler))]
    public class Player : CombatableCharacter
    {
        private ILevelable m_Levelable;
        [SerializeField] private Transform m_Target;
        private NavMeshAgent m_NavMeshAgent;
        private AbilityHandler _abilityHandler;


        protected override void Awake()
        {
            base.Awake();
            m_Levelable = GetComponent<ILevelable>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            _abilityHandler = GetComponent<AbilityHandler>();
        }

        private void Update()
        {
            
        }
    }
}