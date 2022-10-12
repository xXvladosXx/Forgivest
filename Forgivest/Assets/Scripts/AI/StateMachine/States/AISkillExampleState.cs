using UnityEngine;

namespace AI.StateMachine.States
{
    public class AISkillExampleState : AISkillBaseState
    {

        public AISkillExampleState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();
            if (Time.deltaTime < SkillCooldown)
            {
                Debug.Log("State is in cooldown");
                AIStateMachine.ChangeState(AIStateMachine.AIIdleEnemyState);
            }
            else
            {
                Debug.Log("Die...");
                AIStateMachine.AIEnemy.AnimationChanger.StartAnimation(
                    AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.SkillParameterHash);

            }

        }

        public override void OnAnimationExitEvent()
        {
            Cooldown(AIStateMachine.AIEnemy.Config.Skill1CooldownTime);
            AIStateMachine.ChangeState(AIStateMachine.AIIdleEnemyState);
            base.OnAnimationExitEvent();
        }

        public override void Exit()
        {
            base.Exit();
            AIStateMachine.AIEnemy.AnimationChanger.StopAnimation(
                AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.SkillParameterHash);
        }

    }
}
