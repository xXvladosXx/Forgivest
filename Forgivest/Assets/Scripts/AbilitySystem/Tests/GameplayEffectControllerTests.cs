using System.Collections;
using System.Linq;
using AbilitySystem.AbilitySystem.Runtime;
using AbilitySystem.AbilitySystem.Runtime.Effects.Stackable;
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
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var statController = _player.GetComponent<StatController>();
            var damageEffectDefinition = AssetDatabase.LoadAssetAtPath<GameplayEffectDefinition>("Assets/Tests/SO/Test_GameplayEffect.asset");
            var damageEffect = new GameplayEffect(damageEffectDefinition, null, _enemy);
            Health health = statController.Stats["Health"] as Health;
            health.currentValue.Should().Be(100);
            effectController.ApplyGameplayEffectToSelf(damageEffect);
            health.currentValue.Should().Be(90);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPersistentEffectApplied_AddStatModifier()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var statController = _player.GetComponent<StatController>();
            var testEffectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/Tests/SO/Test_GameplayPersistentEffect.asset");

            var testEffect = new GameplayPersistentEffect(testEffectDefinition, null, _player);
            var intelligence = statController.Stats["Intelligence"];
            intelligence.Value.Should().Be(1);
            effectController.ApplyGameplayEffectToSelf(testEffect);
            intelligence.Value.Should().Be(4);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPersistentEffectExpires_RemoveStatModifier()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var statController = _player.GetComponent<StatController>();
            var testEffectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/Tests/SO/Persistance/Test_TimeableGameplayPersistentEffect.asset");

            var testEffect = new GameplayPersistentEffect(testEffectDefinition, null, _player);
            var intelligence = statController.Stats["Intelligence"];
            intelligence.Value.Should().Be(1);
            effectController.ApplyGameplayEffectToSelf(testEffect);
            intelligence.Value.Should().Be(4);
            yield return new WaitForSeconds(4);
            intelligence.Value.Should().Be(1);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenStart_AppliesStartingEffects()
        {
            yield return null;
            var statController = _player.GetComponent<StatController>();
            var dexterity = statController.Stats["Dexterity"];
            dexterity.Value.Should().Be(4);
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenEffectApplied_AddGrantedTags()
        {
            yield return null;
            var tagController = _player.GetComponent<TagController>();
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var persistentEffectDefinition = AssetDatabase
                .LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/Tests/SO/GrantedTags/GameplayPersistentEffect.asset");
            var persistentEffect = new GameplayPersistentEffect(persistentEffectDefinition, null, _player);
            effectController.ApplyGameplayEffectToSelf(persistentEffect);
            tagController.Tags.Should().Contain("Test");
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPersistentEffectExpires_RemoveGrantedTags()
        {
            yield return null;
            var tagController = _player.GetComponent<TagController>();
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var persistentEffectDefinition = AssetDatabase
                .LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/Tests/SO/GrantedTags/GameplayPersistentEffect.asset");
            var persistentEffect = new GameplayPersistentEffect(persistentEffectDefinition, null, _player);
            effectController.ApplyGameplayEffectToSelf(persistentEffect);
            
            yield return new WaitForSeconds(2);

            tagController.Tags.Should().NotContain("Test");
            tagController.Tags.Count.Should().Be(0);
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPeriodReached_ExecuteGameplayEffect()
        {
            yield return null;
            var effectContrller = _player.GetComponent<GameplayEffectHandler>();
            var statController = _player.GetComponent<StatController>();
            var effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                "Assets/Tests/SO/PeriodicEffects/Period/GameplayPersistentEffect.asset");

            var health = statController.Stats["Health"] as Health;
            var effect = new GameplayPersistentEffect(effectDefinition, null, _player);
            effectContrller.ApplyGameplayEffectToSelf(effect);
            health.currentValue.Should().Be(100);
            yield return new WaitForSeconds(1);
            health.currentValue.Should().Be(110);
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenApplied_ExecutePeriodicGameplayEffect()
        {
            yield return null;
            var effectContrller = _player.GetComponent<GameplayEffectHandler>();
            var statController = _player.GetComponent<StatController>();
            var effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                "Assets/Tests/SO/PeriodicEffects/Instant/GameplayPersistentEffect.asset");

            var health = statController.Stats["Health"] as Health;
            var effect = new GameplayPersistentEffect(effectDefinition, null, _player);
            health.currentValue.Should().Be(100);
            effectContrller.ApplyGameplayEffectToSelf(effect);
            health.currentValue.Should().Be(110);
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenOverflow_AppliesOverflowEffects()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var statController = _player.GetComponent<StatController>();
            var effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/Tests/SO/OverflowEffects/GameplayStackableEffect.asset");
            
            var health = statController.Stats["Health"] as Health;
            health.currentValue.Should().Be(100);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, _player));
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, _player));
            health.currentValue.Should().Be(95);
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenOverflow_ClearStack()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/Tests/SO/ClearStack/GameplayStackableEffect.asset");

            var stackableEffect = new GameplayStackableEffect(effectDefinition, null, _player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, _player));
            var secondStackableEffect = effectController.GetActiveEffects
                .FirstOrDefault(effect => effect.Definition == effectDefinition) as GameplayStackableEffect;
            stackableEffect.Should().NotBe(secondStackableEffect);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenOverflow_DoNotApplyEffect()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/Tests/SO/DoNotApply/GameplayStackableEffect.asset");
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, _player));
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, _player)).Should().BeFalse();
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenApplyStack_ResetDuration()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/Tests/SO/ResetStack/GameplayStackableEffect.asset");
            var stackableEffect = new GameplayStackableEffect(effectDefinition, null, _player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            yield return new WaitForSeconds(1);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.RemainingDuration, 9f, .1f);
            yield return new WaitForSeconds(1);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, _player));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.RemainingDuration, 10f, .1f);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenApplyStack_ResetPeriod()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/Tests/SO/ResetPeriod/GameplayStackableEffect.asset");
            var stackableEffect = new GameplayStackableEffect(effectDefinition, null, _player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            yield return new WaitForSeconds(2);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.RemainingPeriod, 1f, .1f);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, _player));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.RemainingPeriod, 3f, .1f);
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenApplyStack_IncreaseStackCount()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/Tests/SO/ResetPeriod/GameplayStackableEffect.asset");
            var stackableEffect = new GameplayStackableEffect(effectDefinition, null, _player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, _player));
            stackableEffect.StackCount.Should().Be(2);
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenDurationReached_RemoveStackAndRefresh()
        {
            yield return null;
            var effectController = _player.GetComponent<GameplayEffectHandler>();
            var effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/Tests/SO/RemoveAndRefresh/GameplayStackableEffect.asset");
            var stackableEffect = new GameplayStackableEffect(effectDefinition, null, _player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, _player));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.RemainingDuration, 3f, .1f);
            stackableEffect.StackCount.Should().Be(2);
            yield return new WaitForSeconds(3);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.RemainingDuration, 3f, .1f);
            stackableEffect.StackCount.Should().Be(1);
        }
    }
}