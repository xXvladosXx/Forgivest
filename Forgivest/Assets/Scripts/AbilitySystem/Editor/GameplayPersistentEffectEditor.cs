using AbilitySystem.AbilitySystem.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AbilitySystem
{
    [CustomEditor(typeof(GameplayPersistentEffectDefinition))]
    public class GameplayPersistentEffectEditor : GameplayEffectEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            root.Add(CreateApplicationFieldsGUI());
            root.Add(CreateDurationFieldsGUI());
            root.Add(CreateTagFieldsGUI());

            RegisterCallbacks(root);
            
            return root;
        }

        private VisualElement CreateTagFieldsGUI()
        {
            var root = new VisualElement();
            root.Add(new PropertyField(serializedObject.FindProperty("_grantedTags")));

            return root;
        }

        private void RegisterCallbacks(VisualElement root)
        {
            var definition = target as GameplayPersistentEffectDefinition;
            var durationField = root.Q<PropertyField>("duration-formula");
            var isInfiniteField = root.Q<PropertyField>("is-infinite");

            durationField.style.display = definition.IsInfinite ? DisplayStyle.None : DisplayStyle.Flex;
            isInfiniteField.RegisterValueChangeCallback(evt => 
                durationField.style.display = evt.changedProperty.boolValue ? DisplayStyle.None : DisplayStyle.Flex);
        }

        private VisualElement CreateDurationFieldsGUI()
        {
            var root = new VisualElement();

            root.Add(new PropertyField(serializedObject.FindProperty("_isInfinite"))
            {
                name = "is-infinite"
            });
            
            root.Add(new PropertyField(serializedObject.FindProperty("_durationFormula"))
            {
                name = "duration-formula"
            });

            return root;
        }
    }
}