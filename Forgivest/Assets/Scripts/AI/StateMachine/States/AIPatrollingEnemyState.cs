using System.Collections;
using UnityEngine;

namespace AI.StateMachine.States
{
    public class AIPatrollingEnemyState : AIBaseState
    {
        private Transform _currentWaypoint;
        
        public AIPatrollingEnemyState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
            
        }
        
        
 
        public override void Enter()
        {
            base.Enter();
            if (_currentWaypoint == null)
            {
                _currentWaypoint = AIStateMachine.AIEnemy.Objects[0].transform;
            }
            else
            {
                var range = Random.Range(0, ((ICollection)AIStateMachine.AIEnemy.Objects).Count);
                _currentWaypoint = AIStateMachine.AIEnemy.Objects[range].transform;
            }
            AIStateMachine.AIEnemy.Movement.MoveTo(_currentWaypoint.position, AIStateMachine.AIEnemy.Config.MovementPatrollingSpeed);
        }
        
        public override void Update()
        {
            base.Update();
            if (IsPlayerInSight(AIStateMachine.AIEnemy.Config.AggroSightDistance))
            {
                AIStateMachine.ChangeState(AIStateMachine.AIChasingEnemyState);
                return;
            }
            
            if(GetDistanceToWaypoint() < AIStateMachine.AIEnemy.Config.DistanceToWaypoint)
            {
                AIStateMachine.ChangeState(AIStateMachine.AIIdleEnemyState);
            }
        }

        private float GetDistanceToWaypoint() =>
            AIStateMachine.AIEnemy.Movement.GetDistanceToPoint(_currentWaypoint.position);
    }
}
