using UnityEngine;

namespace Player.AI.StateMachine.States
{
    public class AISecondSkillState : AISkillBaseState
    {
        
        public AISecondSkillState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
            Id = 1;
        }

        public override void Enter()
        {
            CooldownDuration = AIStateMachine.AIEnemy.Config.FirstSkillCooldownTime;
            base.Enter();
            if (AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(Id))
            {
                Debug.Log("State is in cooldown");
                AIStateMachine.ChangeState(AIStateMachine.AIChasingEnemyState);
            }
            else
            {
                Debug.Log("Die...");
                AIStateMachine.AIEnemy.AnimationChanger.StartAnimation(
                    AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.FirstSkillParameterHash);

            }

        }

        public override void OnAnimationExitEvent()
        {
            AIStateMachine.AIEnemy.Cooldown.AddToCooldown(this);
            AIStateMachine.ChangeState(AIStateMachine.AIIdleEnemyState);
            base.OnAnimationExitEvent();
        }

        public override void Exit()
        {
            base.Exit();
            AIStateMachine.AIEnemy.AnimationChanger.StopAnimation(
                AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.FirstSkillParameterHash);
        }

    }
}
