using Player.AI.StateMachine.Core;

namespace Player.AI.StateMachine.States
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
