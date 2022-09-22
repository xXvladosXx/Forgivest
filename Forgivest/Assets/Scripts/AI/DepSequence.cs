using UnityEngine.AI;

namespace AI
{
    public class DepSequence : Node
    {
        BehaviourTree dependancy;
        NavMeshAgent agent;
        public DepSequence(string n, BehaviourTree d, NavMeshAgent a)
        {
            name = n;
            dependancy = d;
            agent = a;
        }

        public override Status Process()
        {
            if (dependancy.Process() == Status.FAILURE)
            {
                agent.ResetPath();
                //reset all children
                foreach (Node n in Children)
                    n.Reset();
                return Status.FAILURE;
            }

            Status childstatus = Children[CurrentChild].Process();
            if (childstatus == Status.RUNNING) return Status.RUNNING;
            if (childstatus == Status.FAILURE)
                return childstatus;

            CurrentChild++;
            if (CurrentChild >= Children.Count)
            {
                CurrentChild = 0;
                return Status.SUCCESS;
            }

            return Status.RUNNING;
        }


    }
}
