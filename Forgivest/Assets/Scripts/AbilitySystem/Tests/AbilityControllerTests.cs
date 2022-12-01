using System.Collections;
using AbilitySystem.AbilitySystem.Runtime;
using AbilitySystem.AbilitySystem.Runtime.Abilities;
using FluentAssertions;
using NUnit.Framework;
using StatsSystem.Scripts.Runtime;
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
            var wisdom = statController.Stats["Wisdom"];
            wisdom.Value.Should().Be(11);
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenActivateAbility_ApplyEffects()
        {
            yield return null;
            var abilityController = _player.GetComponent<AbilityHandler>();
            var statController = _enemy.GetComponent<StatController>();
            var health = statController.Stats["Health"] as Health;
            health.CurrentValue.Should().Be(100);
            abilityController.TryActiveAbility("SingleTargetAbility");
            health.CurrentValue.Should().Be(95);
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenActivateAbility_ApplyCostEffect()
        {
            yield return null;
            var abilityController = _player.GetComponent<AbilityHandler>();
            var statController = _player.GetComponent<StatController>();
            var mana = statController.Stats["Mana"] as Attribute;
            mana.CurrentValue.Should().Be(100);
            abilityController.TryActiveAbility("AbilityWithCost");
            mana.CurrentValue.Should().Be(50);
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenCannotSatisfyActivationAbility_BlockActivation()
        {
            yield return null;
            var abilityController = _player.GetComponent<AbilityHandler>();
            var statController = _player.GetComponent<StatController>();
            var mana = statController.Stats["Mana"] as Attribute;
            mana.CurrentValue.Should().Be(100);
            abilityController.TryActiveAbility("AbilityWithCost");
            abilityController.TryActiveAbility("AbilityWithCost").Should().BeTrue();
            
            abilityController.TryActiveAbility("AbilityWithCost");
            mana.CurrentValue.Should().Be(0);
            abilityController.TryActiveAbility("AbilityWithCost").Should().BeFalse();
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenAbilityOnCooldown_BlockAbilityActivation()
        {
            var tagController = _player.GetComponent<TagRegister>();
            var abilityController = _player.GetComponent<AbilityHandler>();
            abilityController.TryActiveAbility("CooldownAbility");
            tagController.Tags.Should().Contain("test_cooldown");
            bool canBeActivated = abilityController.TryActiveAbility("CooldownAbility");
            canBeActivated.Should().BeFalse();
            yield return new WaitForSeconds(2f);
            canBeActivated = abilityController.TryActiveAbility("CooldownAbility");
            canBeActivated.Should().BeTrue();
        }
        
        
    }
}