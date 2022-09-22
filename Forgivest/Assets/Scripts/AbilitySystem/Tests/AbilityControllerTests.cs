using System.Collections;
using AbilitySystem.AbilitySystem.Runtime;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using FluentAssertions;
using NUnit.Framework;
using StatSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace AbilitySystem.Tests
{
    public class AbilityControllerTests
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
        public IEnumerator AbilityController_WhenStart_ApplyPassiveAbility()
        {
            yield return null;
            var statController = _player.GetComponent<StatController>();
            var wisdom = statController.stats["Wisdom"];
            wisdom.value.Should().Be(11);
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenActivateAbility_ApplyEffects()
        {
            yield return null;
            var abilityController = _player.GetComponent<AbilityController>();
            var statController = _enemy.GetComponent<StatController>();
            var health = statController.stats["Health"] as Health;
            health.currentValue.Should().Be(100);
            abilityController.TryActiveAbility("SingleTargetAbility", _enemy);
            health.currentValue.Should().Be(95);
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenActivateAbility_ApplyCostEffect()
        {
            yield return null;
            var abilityController = _player.GetComponent<AbilityController>();
            var statController = _player.GetComponent<StatController>();
            var mana = statController.stats["Mana"] as Attribute;
            mana.currentValue.Should().Be(100);
            abilityController.TryActiveAbility("AbilityWithCost", _enemy);
            mana.currentValue.Should().Be(50);
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenCannotSatisfyActivationAbility_BlockActivation()
        {
            yield return null;
            var abilityController = _player.GetComponent<AbilityController>();
            var statController = _player.GetComponent<StatController>();
            var mana = statController.stats["Mana"] as Attribute;
            mana.currentValue.Should().Be(100);
            abilityController.TryActiveAbility("AbilityWithCost", _enemy);
            abilityController.TryActiveAbility("AbilityWithCost", _enemy).Should().BeTrue();
            
            abilityController.TryActiveAbility("AbilityWithCost", _enemy);
            mana.currentValue.Should().Be(0);
            abilityController.TryActiveAbility("AbilityWithCost", _enemy).Should().BeFalse();
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenAbilityOnCooldown_BlockAbilityActivation()
        {
            var tagController = _player.GetComponent<TagController>();
            var abilityController = _player.GetComponent<AbilityController>();
            abilityController.TryActiveAbility("CooldownAbility", _enemy);
            tagController.tags.Should().Contain("test_cooldown");
            bool canBeActivated = abilityController.TryActiveAbility("CooldownAbility", _player);
            canBeActivated.Should().BeFalse();
            yield return new WaitForSeconds(2f);
            canBeActivated = abilityController.TryActiveAbility("CooldownAbility", _player);
            canBeActivated.Should().BeTrue();
        }
    }
}