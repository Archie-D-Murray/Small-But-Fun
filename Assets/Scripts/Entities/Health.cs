using System;
using System.Linq;

using UnityEngine;

using System.Collections;

using Utilities;

namespace Entity {
    public class Health : MonoBehaviour {
        public float PercentHealth => Mathf.Clamp01(_currentHealth / _maxHealth);
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public bool Invulnerable => _invulnerable;
        public Action<float> OnDamage;
        public Action<float> OnHeal;
        public Action OnDeath;
        public Action OnInvulnerableDamage;

        private float _invulnerabilityTimer = 0f;
        private bool _invulnerable = false;
        private Coroutine _invulnerabilityReset = null;

        [SerializeField] private float _currentHealth;
        [SerializeField] private float _maxHealth;

        private void Awake() {
            _currentHealth = _maxHealth;
        }

        private void UpdateMaxHealth(float health) {
            Debug.Log($"Updated max health for {name} to {health}");
            float diff = health - _currentHealth;
            _maxHealth = health;
            Heal(diff);
        }

        /// <summary>Damages an entity</summary>
        /// <param name="damage">Damage to apply</param>
        /// <param name="entityPos">Position of entity that applied the knockback</param>
        public void Damage(float damage, Vector2? entityPos = null) {
            if (_currentHealth == 0.0f) { //Don't damage dead things!
                return;
            } else if (_invulnerable) {
                OnInvulnerableDamage?.Invoke();
                return;
            }
            damage = Mathf.Max(damage, 0.0f);
            if (damage != 0.0f) {
                _currentHealth = Mathf.Max(0.0f, _currentHealth - damage);
                OnDamage?.Invoke(damage);
            }
            if (_currentHealth == 0.0f) {
                Debug.Log($"{name} is dead");
                OnDeath?.Invoke();
            }
        }

        public void Heal(float amount) {
            amount = Mathf.Max(0f, amount);
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
            OnHeal?.Invoke(amount);
        }

        public void SetInvulnerable(float time) {
            if (_invulnerabilityReset != null) {
                StopCoroutine(_invulnerabilityReset);
            }
            _invulnerabilityTimer += time;
            _invulnerabilityReset = StartCoroutine(InvulnerabilityReset());
        }

        public void SetInvulnerable(bool invulnerable) {
            this._invulnerable = invulnerable;
        }

        private IEnumerator InvulnerabilityReset() {
            while (_invulnerabilityTimer >= 0f) {
                _invulnerabilityTimer -= Time.fixedDeltaTime;
                yield return Yielders.WaitForFixedUpdate;
            }
        }
    }
}