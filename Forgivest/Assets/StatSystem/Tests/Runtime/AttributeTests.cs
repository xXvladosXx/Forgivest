﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace StatSystem.Tests
{
    public class AttributeTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatSystem/Tests/Scenes/Test.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator Attribute_WhenModifierApplied_DoesNotExceedMaxValue()
        {
            yield return null;
            StatController statController = GameObject.FindObjectOfType<StatController>();
            Attribute health = statController.stats["Health"] as Attribute;
            Assert.AreEqual(100, health.currentValue);
            Assert.AreEqual(100, health.value);
            health.ApplyModifier(new StatModifier
            {
                Magnitude = 20,
                Type = ModifierOperationType.Additive
            });
            Assert.AreEqual(100, health.currentValue);
        }

        [UnityTest]
        public IEnumerator Attribute_WhenModifierApplied_DoesNotGoBelowZero()
        {
            yield return null;
            StatController statController = GameObject.FindObjectOfType<StatController>();
            Attribute health = statController.stats["Health"] as Attribute;
            Assert.AreEqual(100, health.currentValue);
            health.ApplyModifier(new StatModifier
            {
                Magnitude = -150,
                Type = ModifierOperationType.Additive
            });
            Assert.AreEqual(0, health.currentValue);
        }

        [UnityTest]
        public IEnumerator Attribute_WhenTakeDamage_DamageReducedByDefense()
        {
            yield return null;
            StatController statController = GameObject.FindObjectOfType<StatController>();
            Health health = statController.stats["Health"] as Health;
            Assert.AreEqual(100, health.currentValue);
            health.ApplyModifier(new HealthModifier
            {
                Magnitude = -10,
                Type = ModifierOperationType.Additive,
                IsCriticalHit = false,
                Source = ScriptableObject.CreateInstance<Ability>()
            });
            Assert.AreEqual(95, health.currentValue);
        }

        private class Ability : ScriptableObject, ITaggable
        {
            private List<string> m_Tags = new List<string>() { "physical" };
            public ReadOnlyCollection<string> Tags => m_Tags.AsReadOnly();
        }
    }
}