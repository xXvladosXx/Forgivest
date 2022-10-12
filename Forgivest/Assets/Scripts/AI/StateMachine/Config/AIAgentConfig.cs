using Data.Player;
using UnityEngine;

namespace AI.StateMachine.Config
{
    [CreateAssetMenu( fileName = "AIConfig", menuName = "AI/State" )]
    public class AIAgentConfig : ScriptableObject
    {
        public float maxTime = 1.0f;
        public float maxDistance = 1.0f;
        public float maxSightDistance = 5.0f;
        [field: SerializeField] public float DistanceToTarget { get; private set; } = 1.0f;
        [field: SerializeField] public float DistanceToWaypoint { get; private set; } = 0.5f;
        
        [field: SerializeField] public float MovementPatrollingSpeed { get; private set; } = 3.0f;
        
        [field: SerializeField] public float MovementChasingSpeed { get; private set; } = 7.0f;
        [field: SerializeField] public float IdleTime { get; private set; }
        [field: SerializeField] public float AggroSightDistance { get; private set; } = 3.0f;
        
        [field: SerializeField] public float ChaseDistance { get; private set; } = 5.0f;
        
        [field: SerializeField] public float DistanceToAttack { get; private set; } = 2.0f;
        
        [field: SerializeField] public float TimeDelayBeforeAttack { get; private set; } = 1.5f;
        
        [field: SerializeField] public float Skill1CooldownTime { get; private set; } = 5f;
        
        [field: SerializeField] public float Skill2CooldownTime { get; private set; } = 3f;
        
    }
}
