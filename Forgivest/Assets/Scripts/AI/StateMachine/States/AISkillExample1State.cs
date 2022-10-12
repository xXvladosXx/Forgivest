using UnityEngine;

namespace AI.StateMachine.States
{
    public class AISkillExample1State : AISkillBaseState
    {
        
        public AISkillExample1State(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
            
        }
        public override void Enter()
        {
            base.Enter();
            if(Time.time < SkillCooldown)
            {
                Debug.Log("State is in cooldown");
                AIStateMachine.ChangeState(AIStateMachine.AIIdleEnemyState);
            }
            else{
                Debug.Log("Down Strike!");
                AIStateMachine.AIEnemy.AnimationChanger.StartAnimation(
                    AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.Skill2ParameterHash);
            }
            
        }

        public override void OnAnimationExitEvent()
        {
            Cooldown(AIStateMachine.AIEnemy.Config.Skill2CooldownTime);
            AIStateMachine.ChangeState(AIStateMachine.AIIdleEnemyState);
            base.OnAnimationExitEvent();
        }

        public override void Exit()
        {
            base.Exit();
            AIStateMachine.AIEnemy.AnimationChanger.StopAnimation(
                AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.Skill2ParameterHash);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
