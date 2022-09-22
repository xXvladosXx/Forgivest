using UnityEngine;

namespace Core.Nodes
{
    public class ScalarNode : CodeFunctionNode
    {
        [SerializeField] protected float m_Value;
        public override float Value => m_Value;
        public override float CalculateValue(GameObject source)
        {
            return m_Value;
        }
    }
}