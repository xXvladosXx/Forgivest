using System.Collections;
using AbilitySystem.AbilitySystem.Runtime;
using FluentAssertions;
using NUnit.Framework;
using StatSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace AbilitySystem.Tests
{
    public class GameplayEffectControllerTests
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
            _enemy = GameObject.Instantiate(_enemyPrefab);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenEffectApplied_ModifyAttribute()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectController>();
            var statController = _player.GetComponent<StatController>();
            var damageEffectDefinition = AssetDatabase.LoadAssetAtPath<GameplayEffectDefinition>("Assets/Tests/SO/Test_GameplayEffect.asset");
            var damageEffect = new GameplayEffect(damageEffectDefinition, null, _enemy);
            Health health = statController.stats["Health"] as Health;
            health.currentValue.Should().Be(100);
            effectController.ApplyGameplayEffectToSelf(damageEffect);
            health.currentValue.Should().Be(90);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPersistentEffectApplied_AddStatModifier()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectController>();
            var statController = _player.GetComponent<StatController>();
            var testEffectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/Tests/SO/Test_GameplayPersistentEffect.asset");

            var testEffect = new GameplayPersistentEffect(testEffectDefinition, null, _player);
            var intelligence = statController.stats["Intelligence"];
            intelligence.value.Should().Be(1);
            effectController.ApplyGameplayEffectToSelf(testEffect);
            intelligence.value.Should().Be(4);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPersistentEffectExpires_RemoveStatModifier()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectController>();
            var statController = _player.GetComponent<StatController>();
            var testEffectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/Tests/SO/Persistance/Test_TimeableGameplayPersistentEffect.asset");

            var testEffect = new GameplayPersistentEffect(testEffectDefinition, null, _player);
            var intelligence = statController.stats["Intelligence"];
            intelligence.value.Should().Be(1);
            effectController.ApplyGameplayEffectToSelf(testEffect);
            intelligence.value.Should().Be(4);
            yield return new WaitForSeconds(4);
            intelligence.value.Should().Be(1);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenStart_AppliesStartingEffects()
        {
            yield return null;
            var statController = _player.GetComponent<StatController>();
            var dexterity = statController.stats["Dexterity"];
            dexterity.value.Should().Be(4);
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenEffectApplied_AddGrantedTags()
        {
            yield return null;
            var tagController = _player.GetComponent<TagController>();
            var effectController = _player.GetComponent<GameplayEffectController>();
            var persistentEffectDefinition = AssetDatabase
                .LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/Tests/SO/GrantedTags/GameplayPersistentEffect.asset");
            var persistentEffect = new GameplayPersistentEffect(persistentEffectDefinition, null, _player);
            effectController.ApplyGameplayEffectToSelf(persistentEffect);
            tagController.tags.Should().Contain("Test");
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPersistentEffectExpires_RemoveGrantedTags()
        {
            yield return null;
            var tagController = _player.GetComponent<TagController>();
            var effectController = _player.GetComponent<GameplayEffectController>();
            var persistentEffectDefinition = AssetDatabase
                .LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/Tests/SO/GrantedTags/GameplayPersistentEffect.asset");
            var persistentEffect = new GameplayPersistentEffect(persistentEffectDefinition, null, _player);
            effectController.ApplyGameplayEffectToSelf(persistentEffect);
            
            yield return new WaitForSeconds(2);

            tagController.tags.Should().NotContain("Test");
            tagController.tags.Count.Should().Be(0);
        }
        
        
    }
}