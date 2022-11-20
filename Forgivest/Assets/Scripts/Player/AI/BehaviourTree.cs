using System.Collections.Generic;
using UnityEngine;

namespace Player.AI
{
    public class BehaviourTree : Node
    {
        public BehaviourTree()
        {
            name = "Tree";
        }

        public BehaviourTree(string n)
        {
            name = n;
        }

        public override Status Process()
        {
            if (Children.Count == 0) return Status.SUCCESS;
            return Children[CurrentChild].Process();
        }


        struct NodeLevel
        {
            public int level;
            public Node node;
        }

        public void PrintTree()
        {
            string treePrintout = "";
            Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
            Node currentNode = this;
            nodeStack.Push(new NodeLevel { level = 0, node = currentNode } );

            while (nodeStack.Count != 0)
            {
                NodeLevel nextNode = nodeStack.Pop();
                treePrintout += new string('-', nextNode.level) + nextNode.node.name + "\n";
                for (int i = nextNode.node.Children.Count - 1; i >= 0; i--)
                {
                    nodeStack.Push( new NodeLevel { level = nextNode.level + 1, node = nextNode.node.Children[i] });
                }
            }

            Debug.Log(treePrintout);

        }

    }
}
