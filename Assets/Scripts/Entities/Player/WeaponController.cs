using System;
using System.Collections.Generic;

using Audio;

using UI;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

using Utilities;

namespace Entity.Player {
    public class WeaponController : MonoBehaviour {
        [SerializeField] private float _attackRange = 2;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _attackTime = 1f;
        [SerializeField] private CountDownTimer _attackTimer = new CountDownTimer(0f);
        [SerializeField] private float _damageModifier = 1f;
        private Animator _animator;
        private SizeController _sizeController;
        private Health _health;
        private SpriteRenderer _spriteRenderer;
        private Material _normalMaterial;
        private SFXEmitter _emitter;

        private float damage => _damage * _sizeController.DamageModifier * _damageModifier;
        private float attackRange => _attackRange * _sizeController.DamageModifier;
        private float attackTime => _sizeController.DamageModifier / _attackTime;

        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly int _death = Animator.StringToHash("Death");

        private void Start() {
            _emitter = GetComponent<SFXEmitter>();
            _health = GetComponent<Health>();
            _health.OnDeath += () => {
                _animator.Play(_death);
                PlayerUI.Instance.ShowDeathScreen();
                _emitter.Play(SoundEffectType.Death);
            };
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _normalMaterial = _spriteRenderer.material;
            _health.OnDamage += (float _) => {
                _spriteRenderer.FlashDamage(AssetServer.Instance.FlashMaterial, _normalMaterial, 0.5f, this);
                _emitter.Play(SoundEffectType.Hit);
            };
            _animator = GetComponentInChildren<Animator>();
            _sizeController = GetComponent<SizeController>();
            _attackTimer.OnTimerStart += () => _animator.speed = 1f;
        }

        private void Update() {
            _attackTimer.Update(Time.deltaTime);
            if (Input.GetMouseButtonDown(0) && _attackTimer.IsFinished) {
                _attackTimer.Reset(attackTime);
                _attackTimer.Start();
                _animator.Play(_attack);
                _animator.speed = 1f / attackTime;
                _emitter.Play(SoundEffectType.Attack);
            }
            float fAngle = Mathf.RoundToInt(Helpers.Instance.AngleToMouse(transform));
            if (fAngle < 0) { fAngle = 360 + fAngle; }
            int angle = (int)((fAngle + 45.0f) / 90.0f) * 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void FixedUpdate() {
            PlayerUI.Instance.AttackIndicator.fillAmount = _attackTimer.Progress();
            PlayerUI.Instance.HeathBar.fillAmount = _health.PercentHealth;
            PlayerUI.Instance.HealthReadout.text = $"{_health.CurrentHealth:0} / {_health.MaxHealth:0}";
            PlayerUI.Instance.SizeModifier.text = $"{_sizeController.SizeModifier:0%}";
            PlayerUI.Instance.DamageModifier.text = $"{(_damageModifier * _sizeController.DamageModifier):0%}";
            PlayerUI.Instance.SpeedModifier.text = $"{_sizeController.SpeedModifier:0%}";
            PlayerUI.Instance.AmourModifier.text = $"{_sizeController.DamageModifier:0%}";
        }

        public void Attack() {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, attackRange, _mask)) {
                if (!collider.TryGetComponent(out Health health) || Vector2.Dot(((Vector2)collider.transform.position - (Vector2)transform.position).normalized, Helpers.Instance.VectorToMouse(transform.position)) <= 0.5f || !collider.isTrigger) { // Enemies have actual collider so they don't go through walls + trigger collider :(
                    continue;
                } else {
                    health.Damage(damage);
                    Instantiate(PlayerUI.Instance.DamageNumberPrefab, PlayerUI.Instance.WorldCanvas).GetComponent<DamageNumber>().Init(damage, collider.ClosestPoint(transform.position));
                }
            }
        }

        public void DamageModifier(float magnitude) {
            _damageModifier *= magnitude;
        }
    }
}