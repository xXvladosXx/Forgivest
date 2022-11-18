using UnityEngine;

namespace Player.AI.StateMachine.States
{
    public class AIFirstSkillState : AISkillBaseState
    {
        public AIFirstSkillState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
            Id = 2;
        }
        public override void Enter()
        {
            CooldownDuration = AIStateMachine.AIEnemy.Config.SecondSkillCooldownTime;
            base.Enter();
            if(AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(Id))
            {
                Debug.Log("State is in cooldown");
                AIStateMachine.ChangeState(AIStateMachine.AIChasingEnemyState);
            }
            else{
                Debug.Log("Down Strike!");
                AIStateMachine.AIEnemy.AnimationChanger.StartAnimation(
                    AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.SecondSkillParameterHash);
            }
            
        }

        public override void OnAnimationExitEvent()
        {
            AIStateMachine.AIEnemy.Cooldown.AddToCooldown(this);
            AIStateMachine.ChangeState(AIStateMachine.AIChasingEnemyState);
            base.OnAnimationExitEvent();
        }

        public override void Exit()
        {
            base.Exit();
            AIStateMachine.AIEnemy.AnimationChanger.StopAnimation(
                AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.SecondSkillParameterHash);
        }

       
    }
}
