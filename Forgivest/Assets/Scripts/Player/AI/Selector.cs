namespace Player.AI
{
    public class Selector : Node
    {
        public Selector(string n)
        {
            name = n;
        }

        public override Status Process()
        {
            Status childstatus = Children[CurrentChild].Process();
            if (childstatus == Status.RUNNING) return Status.RUNNING;

            if (childstatus == Status.SUCCESS)
            {
                CurrentChild = 0;
                return Status.SUCCESS;
            }

            CurrentChild++;
            if (CurrentChild >= Children.Count)
            {
                CurrentChild = 0;
                return Status.FAILURE;
            }

            return Status.RUNNING;
        }

    }
}
