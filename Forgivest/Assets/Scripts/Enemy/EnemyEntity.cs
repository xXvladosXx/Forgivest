﻿using System;
using System.Collections.Generic;
using AttackSystem.Core;
using AttackSystem.Reward.Core;
using InventorySystem.Core;
using RaycastSystem.Core;
using Sirenix.OdinInspector;
using StatsSystem.Scripts.Runtime;
using UnityEngine;

namespace Enemy
{
    public class EnemyEntity : SerializedMonoBehaviour, IInteractable, IDamageReceiver, IRaycastable
    {
        [field: SerializeField] public List<IRewardable> Rewardables { get; private set; }

        private StatController _statsHandler;
        private DamageHandler _damageHandler;
        
        public List<IRewardable> Rewards => Rewardables;
        public float Health => _statsHandler.Health.CurrentValue;
        public LayerMask LayerMask => gameObject.layer;

        public GameObject GameObject => gameObject == null ? null : gameObject;

        public event Action<AttackData> OnDamageReceived;
        public event Action OnDestroyed;

        private void Awake()
        {
            _statsHandler = GetComponent<StatController>();
            _damageHandler = new DamageHandler(_statsHandler);
        }

        private void OnEnable()
        {
            _damageHandler.OnDied += OnDied;
        }

        private void OnDied(AttackData attackData)
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            _damageHandler.OnDied -= OnDied;
        }

        public void ReceiveDamage(AttackData attackData)
        {
            print("Damaged enemy");
            attackData.DamageReceiver = this;
            _damageHandler.TakeDamage(attackData);
            OnDamageReceived?.Invoke(attackData);
        }
        
        public void Interact()
        {
        }


        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(IRaycastUser raycastUser)
        {
            return true;
        }
    }
}