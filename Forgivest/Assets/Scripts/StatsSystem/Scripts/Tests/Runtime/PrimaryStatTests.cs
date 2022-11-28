using System.Collections;
using NUnit.Framework;
using StatsSystem.Scripts.Runtime;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace StatsSystem.Scripts.Tests.Runtime
{
    public class PrimaryStatTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatSystem/Tests/Scenes/Test.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator Stat_WhenAddCalled_ChangesBaseValue()
        {
            yield return null;
            StatController statController = GameObject.FindObjectOfType<StatController>();
            PrimaryStat strength = statController.Stats["Strength"] as PrimaryStat;
            Assert.AreEqual(1, strength.Value);
            strength.Add(1);
            Assert.AreEqual(2, strength.Value);
        }
    }
}