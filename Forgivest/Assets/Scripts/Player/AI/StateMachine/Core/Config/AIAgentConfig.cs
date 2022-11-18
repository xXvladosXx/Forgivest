using UnityEngine;

namespace Player.AI.StateMachine.Core.Config
{
    [CreateAssetMenu( fileName = "AIConfig", menuName = "AI/State" )]
    public class AIAgentConfig : ScriptableObject
    {
        [field: SerializeField] public float DistanceToWaypoint { get; private set; } = 0.5f;
        
        [field: SerializeField] public float MovementPatrollingSpeed { get; private set; } = 3.0f;
        
        [field: SerializeField] public float MovementChasingSpeed { get; private set; } = 7.0f;
        [field: SerializeField] public float IdleTime { get; private set; }
        [field: SerializeField] public float AggroSightDistance { get; private set; } = 3.0f;
        
        [field: SerializeField] public float ChaseDistance { get; private set; } = 15.0f;
        [field: SerializeField] public float DistanceToAttack { get; private set; } = 2.0f;
        
        [field: SerializeField] public float DistanceToFirstSkill { get; private set; } = 7.0f;
        
        [field: SerializeField] public float DistanceToSecondSkill { get; private set; } = 4.0f;
        
        [field: SerializeField] public float TimeDelayBeforeAttack { get; private set; } = 1.5f;
        
        [field: SerializeField] public float FirstSkillCooldownTime { get; private set; } = 5f;
        [field: SerializeField] public float SecondSkillCooldownTime { get; private set; } = 3f;
        
  
        
    }
}
