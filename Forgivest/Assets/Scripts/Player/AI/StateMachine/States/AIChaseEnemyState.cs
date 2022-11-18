using UnityEngine;

namespace Player.AI.StateMachine.States
{
    public class AIChaseEnemyState : AIBaseState
    {
        private float _currentAttackRate = 2f;
        private float _attackRate = 2f;


        public AIChaseEnemyState(AIStateMachine aiStateMachine) : base(aiStateMachine)
        {
        }

        public override void Update()
        {
            base.Update();

            if (IsPlayerInSight(AIStateMachine.AIEnemy.Config.ChaseDistance))
            {
                AIStateMachine.AIEnemy.Movement.MoveTo(
                    AIStateMachine.AIEnemy.Target.transform.position,
                    AIStateMachine.AIEnemy.Config.MovementChasingSpeed);
                
                var distanceToAttack = FindDistanceToAttack();

                if (GetPlayerDistance() < distanceToAttack)
                {
                    _currentAttackRate -= Time.deltaTime;
                    AIStateMachine.AIEnemy.Movement.Stop();

                    if (!(_currentAttackRate <= 0)) return;

                    if (AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(2) &&
                        AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(1))
                    {
                        _currentAttackRate = _attackRate;

                        AIStateMachine.ChangeState(AIStateMachine.AIAttackingEnemyState);
                        return;
                    }

                    CastSkill();
                }
            }
            else
            {
                AIStateMachine.ChangeState(AIStateMachine.AIIdleEnemyState);
            }
        }

        private void CastSkill()
        {
            if (SkillSelector == 0)
            {
                _currentAttackRate = _attackRate;

                AIStateMachine.ChangeState(AIStateMachine.AISecondSkillState);
            }
            else
            {
                _currentAttackRate = _attackRate;

                AIStateMachine.ChangeState(AIStateMachine.AIFirstSkillState);
            }
        }

        private float FindDistanceToAttack()
        {
            float distanceToAttack;
            if (AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(2) &&
                AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(1))
            {
                distanceToAttack = AIStateMachine.AIEnemy.Config.DistanceToAttack;
            }
            else
            {
                SkillSelector = Random.Range(0, 2);
                distanceToAttack = SkillSelector == 0
                    ? AIStateMachine.AIEnemy.Config.DistanceToFirstSkill
                    : AIStateMachine.AIEnemy.Config.DistanceToSecondSkill;
            }

            return distanceToAttack;
        }
    }
}