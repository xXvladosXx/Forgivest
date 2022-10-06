namespace StateMachine.Player.States.Attack
{
    public class PlayerChasingState : PlayerAttackState
    {
        public PlayerChasingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        
        public override void Update()
        {
            base.Update();
            if (ShouldStop() && PlayerStateMachine.ReusableData.AttackRate <= 0)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerCombatState);
            } 
        }
        protected override void OnClickPressed()
        {
            base.OnClickPressed();
            
            if (ShouldStop())
            {
                if(PlayerStateMachine.ReusableData.AttackRate <= 0)
                    PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerCombatState);  
                return;    
            }
            
            PlayerStateMachine.Movement.MoveTo(PlayerStateMachine.ReusableData.RaycastClickedPoint, GetMovementSpeed());
        }
        
    }
}