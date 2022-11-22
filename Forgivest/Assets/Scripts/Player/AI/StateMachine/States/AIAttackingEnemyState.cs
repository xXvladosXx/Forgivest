namespace Player.AI.StateMachine.States
{
    public class AIAttackingEnemyState : AIBaseState
    {
        public AIAttackingEnemyState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            AIStateMachine.AIEnemy.Movement.Stop();
            AIStateMachine.AIEnemy.NavMeshAgent.updateRotation = false;
            
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
                
            AIStateMachine.AIEnemy.NavMeshAgent.updateRotation = true;
        }

        public override void Update()
        {
            base.Update();

            var targetToFace = AIStateMachine.AIEnemy.Target.transform.position;
            targetToFace.y = 0;
            
            AIStateMachine.AIEnemy.Movement.Transform.LookAt(targetToFace);
            //Debug.Log("Attacked!");
            if (GetPlayerDistance() > AIStateMachine.AIEnemy.Config.DistanceToAttack)
            {
                AIStateMachine.ChangeState(AIStateMachine.AIChasingEnemyState);
            }
            
        }
        
    }
}
