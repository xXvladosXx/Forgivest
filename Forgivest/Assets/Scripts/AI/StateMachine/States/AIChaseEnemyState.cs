using UnityEngine;

namespace AI.StateMachine.States
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
                    //Under Question
                    float skillDistanceToAttack = AIStateMachine.AIEnemy.Config.DistanceToAttack;
                    if (AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(2) &&
                        AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(1))
                    {
                        skillDistanceToAttack = AIStateMachine.AIEnemy.Config.DistanceToAttack;
                    }
                    else
                    {
                        SkillSelector = Random.Range(0, 2);
                        if (SkillSelector == 0)
                            skillDistanceToAttack = AIStateMachine.AIEnemy.Config.DistanceToFirstSkill;
                        else
                            skillDistanceToAttack = AIStateMachine.AIEnemy.Config.DistanceToSecondSkill;
                    }

                    //Under Question
                    if (GetPlayerDistance() < skillDistanceToAttack)
                    {
                        AIStateMachine.AIEnemy.Movement.Stop();
                        _currentAttackRate -= Time.deltaTime;
                        if (_currentAttackRate <= 0)
                        {

                            if (AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(2) &&
                                AIStateMachine.AIEnemy.Cooldown.IsOnCooldown(1))
                            {
                                Debug.Log("Boba");
                                AIStateMachine.ChangeState(AIStateMachine.AIAttackingEnemyState);
                            }
                            else
                            {
                                if (SkillSelector == 0)
                                {
                                    AIStateMachine.ChangeState(AIStateMachine.AISecondSkillState);
                                }
                                else
                                {
                                    AIStateMachine.ChangeState(AIStateMachine.AIFirstSkillState);
                                }

                            }

                            _currentAttackRate = AIStateMachine.AIEnemy.Config.TimeDelayBeforeAttack;
                        }
                    }
                    else
                {
                    AIStateMachine.ChangeState(AIStateMachine.AIIdleEnemyState);
                }
            }
        }
    }
}