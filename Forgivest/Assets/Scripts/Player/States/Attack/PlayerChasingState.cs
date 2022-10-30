﻿using Player.States.Attack;

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

            //TO DO: Add logic for chasing the player
            if (PlayerStateMachine.ReusableData.InteractableObject != null)
            {
                if (PlayerStateMachine.ReusableData.InteractableObject.GameObject == null)
                {
                    PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
                }
                
                if (!GetDistanceTo(PlayerStateMachine.ReusableData.InteractableObject.GameObject.transform.position))
                {
                    PlayerStateMachine.Movement.MoveTo(
                        PlayerStateMachine.ReusableData.InteractableObject.GameObject.transform.position,
                        GetMovementSpeed());
                    return;
                }
                
                if (GetDistanceTo(PlayerStateMachine.ReusableData.InteractableObject.GameObject.transform.position) &&
                    PlayerStateMachine.ReusableData.AttackRate <= 0)
                {
                    PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerCombatState);
                }
            }
            else
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
            }
        }

        protected override void OnClickPressed()
        {
            base.OnClickPressed();

            if (ShouldStop())
            {
                if (PlayerStateMachine.ReusableData.AttackRate <= 0)
                    PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerCombatState);
                return;
            }

            PlayerStateMachine.Movement.MoveTo(PlayerStateMachine.ReusableData.RaycastClickedPoint, GetMovementSpeed());
        }
    }
}