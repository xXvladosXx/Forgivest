using Core.Nodes;
using UnityEngine;

namespace LevelSystem.Nodes
{
    public class LevelNode : CodeFunctionNode
    {
        public ILevelable Levelable;
        public override float Value => Levelable.Level;
        public override float CalculateValue(GameObject source)
        {
            var levelable = source.GetComponent<ILevelable>();
            return levelable.Level;
        }
    }
}