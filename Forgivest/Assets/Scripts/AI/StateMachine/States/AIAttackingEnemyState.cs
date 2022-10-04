using MovementSystem;
using UnityEngine;

namespace AI.StateMachine.States
{
    public class AIAttackingEnemyState : AIBaseState
    {
        
        private float _distanceToTarget = 1;
        
        public AIAttackingEnemyState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            AIStateMachine.AIEnemy.Movement.Stop(); 
            AIStateMachine.AIEnemy.AnimationChanger.StartAnimation(
                AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.AttackParameterHash);
        }

        public override void OnAnimationExitEvent()
        {
            AIStateMachine.ChangeState(AIStateMachine.AIChasingEnemyState);
        }

        public override void Exit()
        {
            base.Exit();
            AIStateMachine.AIEnemy.AnimationChanger.StopAnimation(
                AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.AttackParameterHash);
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("Attacked!");
            if (GetPlayerDistance() > AIStateMachine.AIEnemy.Config.DistanceToAttack)
            {
                AIStateMachine.ChangeState(AIStateMachine.AIChasingEnemyState);
            }
            
        }
        
    }
}
