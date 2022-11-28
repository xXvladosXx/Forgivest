using System;
using System.Linq;
using AbilitySystem.AbilitySystem.Runtime.Effects.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbilitySystem.Editor
{
    [CustomEditor(typeof(GameplayEffectDefinition))]
    public class GameplayEffectEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            var styleSheet = AssetDatabase
                .LoadAssetAtPath<StyleSheet>("Assets/Scripts/AbilitySystem/Editor/GameplayEffectEditor.uss");
            root.styleSheets.Add(styleSheet);
            
            root.Add(CreateCoreFieldsGUI());
            root.Add(CreateApplicationFieldsGUI());
            root.Add(CreateSpecialEffectFieldsGUI());
            root.Add(CreateGameplayEffectFieldsGUI());
            root.Add(CreateTagFieldsGUI());

            return root;
        }

        protected virtual VisualElement CreateCoreFieldsGUI()
        {
            var root = new VisualElement();
            var description = new TextField
            {
                label = "Description",
                bindingPath = "_description",
                multiline = true
            };
            
            description.Bind(serializedObject);
            description.AddToClassList("description");
            root.Add(description);

            return root;
        }
        
        protected virtual VisualElement CreateSpecialEffectFieldsGUI()
        {
            var root = new VisualElement();
            
            root.Add(new PropertyField(serializedObject.FindProperty("_specialEffectDefinition")));

            return root;
        }
        
        protected virtual VisualElement CreateTagFieldsGUI()
        {
            var root = new VisualElement();
            root.Add(new PropertyField(serializedObject.FindProperty("_tags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_removeEffectsWithTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_applicationMustBePresentTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_applicationMustBeAbsentTags")));

            return root;
        }
        
        protected virtual VisualElement CreateGameplayEffectFieldsGUI()
        {
            var root = new VisualElement();
            ListView listView = new ListView
            {
                 bindingPath = "_conditionalEffects",
                 virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                 reorderable = true,
                 showFoldoutHeader = true,
                 showAddRemoveFooter = true,
                 headerTitle = "Conditional Effects"
            };
            
            listView.Bind(serializedObject);
            root.Add(listView);
            
            return root;
        }
        
        protected VisualElement CreateApplicationFieldsGUI()
        {
            var root = new VisualElement();
            var modifiers = new ListView
            {
                bindingPath = "_modifiers",
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                reorderable = true,
                showFoldoutHeader = true,
                showAddRemoveFooter = true,
                headerTitle = "Modifier"
            };

            modifiers.Bind(serializedObject);
            var addButton = modifiers.Q<Button>("unity-list-view__add-button");
            addButton.clicked += AddButtonOnClicked;
            root.Add(modifiers);
            
            return root;
        }

        private void AddButtonOnClicked()
        {
            Type[] types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(BaseGameplayEffectStatModifier)
                    .IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .ToArray();

            if (types.Length > 1)
            {
                var menu = new GenericMenu();
                foreach (var type in types)
                {
                    menu.AddItem(new GUIContent(type.Name), false, delegate
                    {
                        CreateItem(type);
                    });
                }
                
                menu.ShowAsContext();
            }
            else
            {
                CreateItem(types[0]);
            }
        }

        private void CreateItem(Type type)
        {
            var item = CreateInstance(type) as BaseGameplayEffectStatModifier;
            item.name = "Modifier";
            AssetDatabase.AddObjectToAsset(item , target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var modifiers = serializedObject.FindProperty("_modifiers");
            modifiers.GetArrayElementAtIndex(modifiers.arraySize - 1).objectReferenceValue = item;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}