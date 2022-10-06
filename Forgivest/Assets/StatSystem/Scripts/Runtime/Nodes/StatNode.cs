using Core.Nodes;
using UnityEngine;

namespace StatSystem.Nodes
{
    public class StatNode : CodeFunctionNode
    {
        [SerializeField] private string m_StatName;
        public string statName => m_StatName;
        public Stat stat;
        public override float Value => stat.value;
        public override float CalculateValue(GameObject source)
        {
            var statController = source.GetComponent<StatController>();
            return statController.Stats[m_StatName].value;
        }
    }
}