using System;
using AttackSystem.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemy
{
    public class HealthBar : SerializedMonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private IDamageReceiver _damageReceiver;
        
        private Camera _mainCamera;
        private MaterialPropertyBlock _matBlock;

        private float _maxHealth;
        private float _currentHealth;

        private void Awake() {
            meshRenderer = GetComponent<MeshRenderer>();
            _matBlock = new MaterialPropertyBlock();
        }
        private void Start()
        {
            _mainCamera = Camera.main;
            _maxHealth = _damageReceiver.Health;
            meshRenderer.enabled = false;
        }

        private void OnEnable()
        {
            _damageReceiver.OnDamageReceived += OnDamageReceived;
        }

        private void OnDisable()
        {
            _damageReceiver.OnDamageReceived += OnDamageReceived;
        }
        private void Update()
        {
            if (meshRenderer.enabled)
            {
                AlignCamera();
            }
        }

        private void UpdateParams() {
            meshRenderer.GetPropertyBlock(_matBlock);
            _matBlock.SetFloat("_Fill", _currentHealth / _maxHealth);
            meshRenderer.SetPropertyBlock(_matBlock);
        }

        private void AlignCamera() {
            var camXform = _mainCamera.transform;
                var forward = transform.position - camXform.position;
                forward.Normalize();
                var up = Vector3.Cross(forward, camXform.right);
                transform.rotation = Quaternion.LookRotation(forward, up);
        }

        private void OnDamageReceived(AttackData attackData)
        {
            _currentHealth = _damageReceiver.Health;

            if (!(_currentHealth < _maxHealth)) return;
            
            meshRenderer.enabled = true;
            UpdateParams();
        }
    }
}