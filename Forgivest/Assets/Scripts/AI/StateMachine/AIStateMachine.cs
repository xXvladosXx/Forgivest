using AI.StateMachine.Core;
using AI.StateMachine.States;

namespace AI.StateMachine
{
    public class AIStateMachine : global::StateMachine.StateMachine
    {
        public IAIEnemy AIEnemy { get; }
        
        public AIChaseEnemyState AIChasingEnemyState { get; }
        
        public AIIdleEnemyState AIIdleEnemyState { get; }
        
        public AIPatrollingEnemyState AIPatrollingEnemyState { get; }
        
        public AIAttackingEnemyState AIAttackingEnemyState { get; }
        
        public AISkillBaseState AISkillBaseState { get; }
        
        public AISecondSkillState AISecondSkillState { get; }
        public AIFirstSkillState AIFirstSkillState { get; }
        
        public AIStateMachine(IAIEnemy aiEnemy)
        {
            AIEnemy = aiEnemy;
            AIChasingEnemyState = new AIChaseEnemyState(this);
            AIIdleEnemyState = new AIIdleEnemyState(this);
            AIPatrollingEnemyState = new AIPatrollingEnemyState(this);
            AIAttackingEnemyState = new AIAttackingEnemyState(this);
            AISkillBaseState = new AISkillBaseState(this);
            AISecondSkillState = new AISecondSkillState(this);
            AIFirstSkillState = new AIFirstSkillState(this);
        }
    }
}
