using AbilitySystem.AbilitySystem.Runtime.Effects.Stackable;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AbilitySystem.Editor
{
    [CustomEditor(typeof(GameplayStackableEffectDefinition))]
    public class GameplayStackableEffectEditor : GameplayPersistentEffectEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            var styleSheet = AssetDatabase
                .LoadAssetAtPath<StyleSheet>("Assets/Scripts/AbilitySystem/Editor/GameplayEffectEditor.uss");
            root.styleSheets.Add(styleSheet);
            
            root.Add(CreateCoreFieldsGUI());
            root.Add(CreateApplicationFieldsGUI());
            root.Add(CreateDurationFieldsGUI());
            root.Add(CreateSpecialEffectFieldsGUI());
            root.Add(CreateStackingFieldsGUI());
            root.Add(CreateGameplayEffectFieldsGUI());
            root.Add(CreateTagFieldsGUI());
            root.Add(CreatePeriodFieldsGUI());

            RegisterCallbacks(root);
            
            return root;
        }

        private VisualElement CreateStackingFieldsGUI()
        {
            var root = new VisualElement();
            
            root.Add(new PropertyField(serializedObject.FindProperty("_denyOverflowApplication")));
            root.Add(new PropertyField(serializedObject.FindProperty("_clearStackOnOverflow")));
            root.Add(new PropertyField(serializedObject.FindProperty("_stackCountLimit")));
            root.Add(new PropertyField(serializedObject.FindProperty("_stackingDuration")));
            root.Add(new PropertyField(serializedObject.FindProperty("_stackingPeriod")));
            root.Add(new PropertyField(serializedObject.FindProperty("_stackingExpiration")));

            return root;
        }

        protected override VisualElement CreateGameplayEffectFieldsGUI()
        {
            var root = base.CreateGameplayEffectFieldsGUI();

            ListView overflowGameplayEffects = new ListView
            {
                bindingPath = "_overflowEffects",
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                reorderable = true,
                showFoldoutHeader = true,
                showAddRemoveFooter = true,
                headerTitle = "Overflow gameplay effects"
            };
            overflowGameplayEffects.Bind(serializedObject);
            root.Add(overflowGameplayEffects);

            return root;
        }
    }
}