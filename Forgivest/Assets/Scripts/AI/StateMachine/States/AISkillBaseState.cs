using AI.StateMachine.Core;
using UnityEngine;

namespace AI.StateMachine.States
{
    public class AISkillBaseState : AIBaseState, IHasCooldown
    {
        
        public int Id { get; set; }
        public float CooldownDuration { get; set; }
        
        public AISkillBaseState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
            
        }
        
    }
}
