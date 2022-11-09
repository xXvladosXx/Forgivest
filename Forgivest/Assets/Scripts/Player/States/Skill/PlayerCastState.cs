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
            switch (PlayerStateMachine.AbilityHandler.CurrentAbility)
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
            switch (PlayerStateMachine.AbilityHandler.CurrentAbility)
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
            
            if(PlayerStateMachine.AbilityHandler.CurrentAbility == null) return;
            
            PlayerStateMachine.AnimationChanger.StopAnimation((
                (ActiveAbilityDefinition) PlayerStateMachine.AbilityHandler.CurrentAbility
                    .AbilityDefinition).HashAnimation);
        }

        protected void TryToActivateSkill(int index)
        {
            var skill = PlayerStateMachine.AbilityHandler.Hotbar.ItemContainer.Slots[index].Item;
            
            PlayerStateMachine.AbilityHandler.Abilities.TryGetValue("Spikes", out var ability);
            Debug.Log(ability.ToString());
            Debug.Log(ability.AbilityDefinition.ItemDescription);
            
            var activated = skill switch
            {
                ProjectileAbilityDefinition projectileAbility => TryActivateTargetAbility(index, projectileAbility),
                RadiusDamageAbilityDefinition radiusDamageAbility => TryToActivateFreeAbility(index, radiusDamageAbility),
                SingleTargetAbilityDefinition singleTargetAbility => TryActivateTargetAbility(index, singleTargetAbility),
                AttackAbilityDefinition attackAbility => TryToActivateAttackAbility(index, attackAbility),
                null => false,
                _ => throw new ArgumentOutOfRangeException(nameof(skill))
            };
            
            if(!activated)
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }
        
        protected bool TryActivateTargetAbility(int index, ActiveAbilityDefinition activeAbility)
        {
            if (PlayerStateMachine.ReusableData.Raycastable != null)
            {
                if (!activeAbility.SelfCasted &&
                    PlayerStateMachine.ReusableData.Raycastable.GameObject ==
                    PlayerStateMachine.Movement.Transform.gameObject)
                {
                    return false;
                }

                bool canActivate =
                    PlayerStateMachine.AbilityHandler.TryActiveAbility(index);

                if (!canActivate)
                {
                    PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
                    return false;
                }

                PrepareForCast();
                StartAnimating(activeAbility);
                return true;
            }

            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
            return false;
        }

        protected bool TryToActivateFreeAbility(int index, RadiusDamageAbilityDefinition radiusDamageAbility)
        {
            bool canActivate = PlayerStateMachine.AbilityHandler.TryActiveAbility(index);

            if (!canActivate)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
                return false;
            }

            PrepareForCast();
            StartAnimating(radiusDamageAbility);
            return true;
        }

        protected bool TryToActivateAttackAbility(int index, AttackAbilityDefinition attackAbility)
        {
            bool canActivate =
                PlayerStateMachine.AbilityHandler.TryActiveAbility(index);

            if (!canActivate)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
                return false;
            }

            PrepareForCast();
            StartAnimating(attackAbility);
            return true;
        }

        protected override void OnInteractableCheck()
        {
        }
        private void StartAnimating(ActiveAbilityDefinition activeAbilityDefinition)
        {
            PlayerStateMachine.AnimationChanger.StartAnimation(activeAbilityDefinition.HashAnimation);
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