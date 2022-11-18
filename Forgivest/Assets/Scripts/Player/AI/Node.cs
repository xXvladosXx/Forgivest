using System.Collections.Generic;

namespace Player.AI
{
    public class Node
    {
        public enum Status { SUCCESS, RUNNING, FAILURE };
        public Status status;
        public List<Node> Children = new();
        public int CurrentChild = 0;
        public string name;
        public int sortOrder;

        public Node() { }

        public Node(string n)
        {
            name = n;
        }

        public Node(string n, int order)
        {
            name = n;
            sortOrder = order;
        }

        public void Reset()
        {
            foreach (Node n in Children)
                n.Reset();
            CurrentChild = 0;
        }

        public virtual Status Process()
        {
            return Children[CurrentChild].Process();
        }

        public void AddChild(Node n)
        {
            Children.Add(n);
        }
    }
}
