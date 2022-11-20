using Core;
using Core.Editor;
using Core.Editor.Nodes;
using LevelSystem.Scripts.Runtime.Nodes;
using UnityEngine;

namespace LevelSystem.Scripts.Editor
{
    [NodeType(typeof(LevelNode))]
    [Title("Level System", "Level")]
    public class LevelNodeView : NodeView
    {
        public LevelNodeView()
        {
            title = "Level";
            node = ScriptableObject.CreateInstance<LevelNode>();
            output = CreateOutputPort();
        }
    }
}