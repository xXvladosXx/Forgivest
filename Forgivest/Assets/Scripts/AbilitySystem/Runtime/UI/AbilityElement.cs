using UnityEngine;
using UnityEngine.UIElements;

namespace AbilitySystem.AbilitySystem.Runtime.UI
{
    public class AbilityElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<AbilityElement, UxmlTraits>{}

        public AbilityElement()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("AbilitySystem/UI");
            visualTree.CloneTree(this);
        }
    }
}