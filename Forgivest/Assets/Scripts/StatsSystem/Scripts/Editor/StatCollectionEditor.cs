using Core.Editor;
using StatsSystem.Scripts.Runtime;
using UnityEngine.UIElements;

namespace StatsSystem.Editor
{
    public class StatCollectionEditor : ScriptableObjectCollectionEditor<StatDefinition>
    {
        public new class UxmlFactory : UxmlFactory<StatCollectionEditor, UxmlTraits> {}
    }
}