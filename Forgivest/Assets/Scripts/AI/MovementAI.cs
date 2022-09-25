using UnityEngine;

namespace AI
{
    public class MovementAI : BTAgent
    {
        private float _distanceToTarget = 2;
        
        protected Node.Status RunTo(Vector3 location, float distance)
        {
            if (state == ActionState.IDLE)
            {
                rememberedLocation = location;
            }
            return GoToLocation(rememberedLocation);
        }
        protected Node.Status GoToLocation(Vector3 destination)
        {
            float distanceToTarget = Vector3.Distance(destination, this.transform.position);
            if (state == ActionState.IDLE)
            {
                agent.SetDestination(destination);
                state = ActionState.WORKING;
            }
            else if (Vector3.Distance(agent.pathEndPosition, destination) >= _distanceToTarget)
            {
                state = ActionState.IDLE;
                return Node.Status.FAILURE;
            }
            else if (distanceToTarget < _distanceToTarget)
            {
                state = ActionState.IDLE;
                return Node.Status.SUCCESS;
            }
            return Node.Status.RUNNING;
        }
    }
}
