using UnityEngine;

namespace AI.StateMachine.States
{
    public class AIChaseEnemyState : AIBaseState
    {
        private float _currentAttackRate = 2f;
        private float _attackRate = 2f;
        public AIChaseEnemyState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
        }
        
        public override void Update()
        {
            base.Update();
            
            if (IsPlayerInSight(AIStateMachine.AIEnemy.Config.ChaseDistance))
            {
                AIStateMachine.AIEnemy.Movement.MoveTo(
                    AIStateMachine.AIEnemy.Target.transform.position,
                    AIStateMachine.AIEnemy.Config.MovementChasingSpeed);
                if (GetPlayerDistance() < AIStateMachine.AIEnemy.Config.DistanceToAttack)
                {
                    AIStateMachine.AIEnemy.Movement.Stop(); 
                    _currentAttackRate -= Time.deltaTime;
                    if(_currentAttackRate <= 0)
                    {
                        AIStateMachine.ChangeState(AIStateMachine.AIAttackingEnemyState);
                        _currentAttackRate = AIStateMachine.AIEnemy.Config.TimeDelayBeforeAttack;
                    }
                }
            }
            else
            {
                AIStateMachine.ChangeState(AIStateMachine.AIIdleEnemyState);
            }
        }
    }
}