using UnityEngine;

namespace Core.Nodes
{
    public abstract class CodeFunctionNode : AbstractNode
    {
        public abstract float Value { get; }
        public abstract float CalculateValue(GameObject source);
    }
}