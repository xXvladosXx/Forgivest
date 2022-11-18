using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Player.AI
{
    public class BTAgent : MonoBehaviour
    {
        public BehaviourTree tree;
        public NavMeshAgent agent;

        public enum ActionState { IDLE, WORKING };
        public ActionState state = ActionState.IDLE;

        public Node.Status treeStatus = Node.Status.RUNNING;

        WaitForSeconds waitForSeconds;
        protected Vector3 rememberedLocation;

        // Start is called before the first frame update
        public virtual void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            tree = new BehaviourTree();
            waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1f));
            StartCoroutine(nameof(Behave));
        }

        public Node.Status CanSee(Vector3 target, string tag, float distance, float maxAngle)
        {
            var transform1 = this.transform;
            Vector3 directionToTarget = target - transform1.position;
            if (directionToTarget.magnitude > distance)
            {
                return Node.Status.FAILURE;
            }

            Vector3 agentDirection = agent.transform.forward;
            
            directionToTarget.Normalize();

            float dotProduct = Vector3.Dot(directionToTarget, agentDirection);
            if (dotProduct > 0.0f)
            {
               return Node.Status.SUCCESS;
            }
         
            return Node.Status.FAILURE;
        }
    
        
        

        IEnumerator Behave()
        {
            while (true)
            {
                treeStatus = tree.Process();
                yield return waitForSeconds;
            }
        }
    }
}
