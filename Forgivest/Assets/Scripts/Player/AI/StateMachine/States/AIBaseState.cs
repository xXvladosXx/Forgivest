using AnimatorStateMachine.StateMachine;
using UnityEngine;

namespace AI.StateMachine.States
{
    public class AIBaseState : IState
    {
        public AIStateMachine AIStateMachine { get; }


        public int SkillSelector = Random.Range(0, 1);
        public AIBaseState(AIStateMachine aiStateMachine)
        {
            AIStateMachine = aiStateMachine;
        }
        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
           
        }

        public virtual void Update()
        {
            UpdateMovementAnimation();
        }

        public virtual void FixedUpdate()
        {
           
        }

        public virtual void OnAnimationEnterEvent()
        {
            
        }

        public virtual void OnAnimationExitEvent()
        {
            
        }

        public virtual void OnAnimationTransitionEvent()
        {
           
        }
        
        private void UpdateMovementAnimation()
        {
            Vector3 velocity = AIStateMachine.AIEnemy.Movement.GetNavMeshVelocity();
            Vector3 localVelocity = AIStateMachine.AIEnemy.Movement.Transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;

            AIStateMachine.AIEnemy.AnimationChanger.UpdateBlendAnimation(
                AIStateMachine.AIEnemy.AnimationEventUser.AliveEntityAnimationData.SpeedParameterHash,
                speed, .1f);
        }
        
        protected virtual bool IsPlayerInSight(float distance) => GetPlayerDistance() < distance;

        protected float GetPlayerDistance()
        {
            return Vector3.Distance(AIStateMachine.AIEnemy.Target.position,
                AIStateMachine.AIEnemy.Enemy.transform.position);
        }
    }
}