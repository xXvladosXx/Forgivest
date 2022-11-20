using Player.AI.StateMachine.Core;
using UnityEngine;

namespace Player.AI.StateMachine.States
{
    public class AIIdleEnemyState : AIBaseState
    {
        private float _currentTime;
        public AIIdleEnemyState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _currentTime = 0;
            
            AIStateMachine.AIEnemy.Movement.Stop();
        }

        public override void Update()
        {
            
            base.Update();
            
            _currentTime += Time.deltaTime;
            
            if (IsPlayerInSight(AIStateMachine.AIEnemy.Config.AggroSightDistance))
            {
                AIStateMachine.ChangeState(AIStateMachine.AIChasingEnemyState);
                return;
            }

            if (_currentTime > AIStateMachine.AIEnemy.Config.IdleTime)
            {
                AIStateMachine.ChangeState(AIStateMachine.AIPatrollingEnemyState);
            }
        }

        public void Update(IAIEnemy agent)
        {
           
        }
    }
}