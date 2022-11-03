using System;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active;
using AbilitySystem.AbilitySystem.Runtime.Abilities.Active.Core;
using StateMachine.Player;
using UnityEngine;

namespace Player.States.Skill
{
    public class PlayerCastState : PlayerAliveState
    {
        public PlayerCastState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            OnAbilityActivated();
            PlayerStateMachine.AnimationChanger.StartAnimation(PlayerStateMachine.AnimationData.SkillParameterHash);
        }

        public override void OnAnimationEnterEvent()
        {
            switch (PlayerStateMachine.AbilityController.CurrentAbility)
            {
                case AttackAbility attackAbility:
                    if (PlayerStateMachine.RaycastUser.RaycastHit != null)
                    {
                        attackAbility.OnEnter(PlayerStateMachine.Movement, PlayerStateMachine.RaycastUser.RaycastHit.Value.point);
                    }
                    break;
                case ProjectileAbility projectileAbility:
                    break;
                case RadiusDamageAbility radiusDamageAbility:
                    break;
                case SingleTargetAbility singleTargetAbility:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnAnimationExitEvent()
        {
            switch (PlayerStateMachine.AbilityController.CurrentAbility)
            {
                case AttackAbility attackAbility:
                    attackAbility.OnExit();
                    break;
                case ProjectileAbility projectileAbility:
                    break;
                case RadiusDamageAbility radiusDamageAbility:
                    break;
                case SingleTargetAbility singleTargetAbility:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }

        public override void Exit()
        {
            base.Exit();

            PlayerStateMachine.AnimationChanger.StopAnimation(PlayerStateMachine.AnimationData.SkillParameterHash);
            PlayerStateMachine.Movement.EnableRotation(true);
            
            if(PlayerStateMachine.AbilityController.CurrentAbility == null) return;
            
            PlayerStateMachine.AnimationChanger.StopAnimation((
                (ActiveAbilityDefinition) PlayerStateMachine.AbilityController.CurrentAbility
                    .AbilityDefinition).HashAnimation);
        }

        protected void TryToActivateSkill(int index)
        {
            var skill = PlayerStateMachine.AbilityController.AbilitiesIndex[index];
            bool activated = skill switch
            {
                ProjectileAbility projectileAbility => TryActivateTargetAbility(index, projectileAbility),
                RadiusDamageAbility radiusDamageAbility => TryToActivateFreeAbility(index, radiusDamageAbility),
                SingleTargetAbility singleTargetAbility => TryActivateTargetAbility(index, singleTargetAbility),
                AttackAbility attackAbility => TryToActivateAttackAbility(index, attackAbility),
                _ => throw new ArgumentOutOfRangeException(nameof(skill))
            };
            
            if(!activated)
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }
        
        protected bool TryActivateTargetAbility(int index, ActiveAbility activeAbility)
        {
            if (PlayerStateMachine.ReusableData.Raycastable != null)
            {
                if (!activeAbility.ActiveAbilityDefinition.SelfCasted &&
                    PlayerStateMachine.ReusableData.Raycastable.GameObject ==
                    PlayerStateMachine.Movement.Transform.gameObject)
                {
                    return false;
                }

                bool canActivate =
                    PlayerStateMachine.AbilityController.TryActiveAbility(index);

                if (!canActivate)
                {
                    PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
                    return false;
                }

                PrepareForCast();
                StartAnimating(index);
                return true;
            }

            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
            return false;
        }

        protected bool TryToActivateFreeAbility(int index, RadiusDamageAbility radiusDamageAbility)
        {
            bool canActivate = PlayerStateMachine.AbilityController.TryActiveAbility(index);

            if (!canActivate)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
                return false;
            }

            PrepareForCast();
            StartAnimating(index);
            return true;
        }

        protected bool TryToActivateAttackAbility(int index, AttackAbility attackAbility)
        {
            bool canActivate =
                PlayerStateMachine.AbilityController.TryActiveAbility(index);

            if (!canActivate)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
                return false;
            }

            PrepareForCast();
            StartAnimating(index);
            return true;
        }

        protected override void OnInteractableCheck()
        {
        }
        private void StartAnimating(int index)
        {
            PlayerStateMachine.AnimationChanger.StartAnimation((
                (ActiveAbilityDefinition) PlayerStateMachine.AbilityController.AbilitiesIndex[index]
                    .AbilityDefinition).HashAnimation);
        }

        private void PrepareForCast()
        {
            PlayerStateMachine.Movement.EnableRotation(false);
            PlayerStateMachine.Movement.Stop();
            PlayerStateMachine.Rotator.RotateToTargetPosition(PlayerStateMachine.ReusableData.HoveredPoint);
        }

        private void OnAbilityActivated()
        {
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.FirstSkill.performed -= OnFirstSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.SecondSkill.performed -= OnSecondSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.ThirdSkill.performed -= OnThirdSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.FourthSkill.performed -= OnFourthSkillPerformed;
            PlayerStateMachine.PlayerInputProvider.PlayerMainActions.FifthSkill.performed -= OnFifthSkillPerformed;
        }
    }
}