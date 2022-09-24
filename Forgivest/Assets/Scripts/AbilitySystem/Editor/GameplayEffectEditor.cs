using System;
using System.Linq;
using AbilitySystem.AbilitySystem.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbilitySystem
{
    [CustomEditor(typeof(GameplayEffectDefinition))]
    public class GameplayEffectEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            root.Add(CreateApplicationFieldsGUI());
            root.Add(CreateSpecialEffectFieldsGUI());

            return root;
        }

        protected virtual VisualElement CreateSpecialEffectFieldsGUI()
        {
            var root = new VisualElement();
            
            root.Add(new PropertyField(serializedObject.FindProperty("_specialEffectDefinition")));

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