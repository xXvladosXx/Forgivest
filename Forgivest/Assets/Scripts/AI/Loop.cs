namespace AI
{
    public class Loop : Node
    {
        BehaviourTree dependancy;
        public Loop(string n, BehaviourTree d)
        {
            name = n;
            dependancy = d;
        }

        public override Status Process()
        {
            if (dependancy.Process() == Status.FAILURE)
            {
                return Status.SUCCESS;
            }

            Status childstatus = Children[CurrentChild].Process();
            if (childstatus == Status.RUNNING) return Status.RUNNING;
            if (childstatus == Status.FAILURE)
                return childstatus;

            CurrentChild++;
            if (CurrentChild >= Children.Count)
            {
                CurrentChild = 0;
            }

            return Status.RUNNING;
        }


    }
}
