using UnityEngine;

namespace AI
{
    public class Sequence : Node
    {
        public Sequence(string n)
        {
            name = n;
        }

        public override Status Process()
        {
            Debug.Log("Sequence: " + name + " " + CurrentChild);
            Status childstatus = Children[CurrentChild].Process();
            if (childstatus == Status.RUNNING) return Status.RUNNING;
            if (childstatus == Status.FAILURE)
            {
                CurrentChild = 0;
                foreach (Node n in Children)
                    n.Reset();
                return Status.FAILURE;
            }

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
