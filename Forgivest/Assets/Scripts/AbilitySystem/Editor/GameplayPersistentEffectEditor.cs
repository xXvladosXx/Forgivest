using AbilitySystem.AbilitySystem.Runtime.Effects.Persistent;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AbilitySystem.Editor
{
    [CustomEditor(typeof(GameplayPersistentEffectDefinition))]
    public class GameplayPersistentEffectEditor : GameplayEffectEditor
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
            root.Add(CreateTagFieldsGUI());
            root.Add(CreatePeriodFieldsGUI());

            RegisterCallbacks(root);
            
            return root;
        }


        protected override VisualElement CreateSpecialEffectFieldsGUI()
        {
            var root = base.CreateSpecialEffectFieldsGUI();
            
            root.Add(new PropertyField(serializedObject.FindProperty("_specialPersistentEffectDefinition")));

            return root;
        }

        protected override VisualElement CreateTagFieldsGUI()
        {
            var root = base.CreateTagFieldsGUI();
            root.Add(new PropertyField(serializedObject.FindProperty("_grantedTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_grantedImmunityTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_persistMustBePresentTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_persistMustBeAbsentTags")));

            return root;
        }

        protected void RegisterCallbacks(VisualElement root)
        {
            var definition = target as GameplayPersistentEffectDefinition;
            var durationField = root.Q<PropertyField>("duration-formula");
            var isInfiniteField = root.Q<PropertyField>("is-infinite");

            durationField.style.display = definition.IsInfinite ? DisplayStyle.None : DisplayStyle.Flex;
            isInfiniteField.RegisterValueChangeCallback(evt =>
            {
                durationField.style.display = evt.changedProperty.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
            });

            var periodFields = root.Q("period");
            var isPeriodicFields = root.Q<PropertyField>("is-periodic");
            periodFields.style.display = definition.IsPeriodic ? DisplayStyle.Flex : DisplayStyle.None;
            
            isPeriodicFields.RegisterValueChangeCallback(evt =>
            {
                periodFields.style.display = evt.changedProperty.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
            });
        }

        protected VisualElement CreateDurationFieldsGUI()
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
        
        protected VisualElement CreatePeriodFieldsGUI()
        {
            var root = new VisualElement();
            var periodFields = new VisualElement { name = "period" };
            
            periodFields.Add(new PropertyField(serializedObject.FindProperty("_isExecutableEffectOnApply")));
            periodFields.Add(new PropertyField(serializedObject.FindProperty("_period")));

            root.Add(new PropertyField(serializedObject.FindProperty("_isPeriodic"))
            {
                name = "is-periodic"
            });
            root.Add(periodFields);

            return root;
        }
    }
}