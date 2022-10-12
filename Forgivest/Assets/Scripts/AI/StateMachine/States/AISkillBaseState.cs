using UnityEngine;

namespace AI.StateMachine.States
{
    public class AISkillBaseState : AIBaseState
    {
        protected float SkillCooldown;
        public AISkillBaseState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
            
        }
        public void Cooldown(float cooldown)
        {
            SkillCooldown = Time.deltaTime + cooldown;
        }
    }
}
