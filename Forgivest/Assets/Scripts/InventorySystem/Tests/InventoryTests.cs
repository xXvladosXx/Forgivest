using System.Collections;
using FluentAssertions;
using InventorySystem.Interaction;
using InventorySystem.Items;
using NUnit.Framework;
using StatsSystem.Scripts.Runtime;
using StatSystem;
using StatSystem.Scripts.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace InventorySystem.Tests
{
    public class InventoryTests
    {
        private GameObject _playerPrefab;
        private GameObject _enemyPrefab;
        
        private GameObject _player;
        private GameObject _enemy;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tests/Prefabs/Player.prefab");
            _enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tests/Prefabs/Enemy.prefab");
        }
        
        [SetUp]
        public void BeforeEachTestSetup()
        {
            _player = GameObject.Instantiate(_playerPrefab);
        }
        
        [UnityTest]
        public IEnumerator ObjectPicker_WhenPickedItem_ApplyModifierToHealth()
        {
            yield return null;
            var statController = _player.GetComponent<StatController>();
            var objectPicker = _player.GetComponent<ObjectPicker>();
            var statsFinder = _player.GetComponent<StatsFinder>();
            var health = statController.Stats["Health"];
            health.Value.Should().Be(200);
            objectPicker.ItemEquipHandler.TryToEquip(objectPicker.PickableItem.Item as StatsableItem);
            health.Value.Should().Be(320);
        }
        
        [UnityTest]
        public IEnumerator ObjectPicker_WhenPickedItem_ApplyModifierToStrength()
        {
            yield return null;
            var statController = _player.GetComponent<StatController>();
            var objectPicker = _player.GetComponent<ObjectPicker>();
            var statsFinder = _player.GetComponent<StatsFinder>();
            var health = statController.Stats["Strength"];
            health.Value.Should().Be(10);
            objectPicker.ItemEquipHandler.TryToEquip(objectPicker.PickableItem.Item as StatsableItem);
            health.Value.Should().Be(22);
        }
        
        [UnityTest]
        public IEnumerator StatsFinder_WhenPickedItem_GetValue()
        {
            yield return null;
            var statController = _player.GetComponent<StatController>();
            var objectPicker = _player.GetComponent<ObjectPicker>();
            var statsFinder = _player.GetComponent<StatsFinder>();
            var health = statController.Stats["Health"];
            health.Value.Should().Be(200);
            objectPicker.ItemEquipHandler.TryToEquip(objectPicker.PickableItem.Item as StatsableItem);
            statsFinder.FindStat("Health").Should().Be(320);
        }
    }
}