using System;

using UnityEngine;

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

        private float damage => _damage * _sizeController.DamageModifier * _damageModifier;
        private float attackRange => _attackRange * _sizeController.DamageModifier;
        private float attackTime => _attackTime / _sizeController.DamageModifier;

        private readonly int _attack = Animator.StringToHash("Attack");

        private void Start() {
            _animator = GetComponentInChildren<Animator>();
            _sizeController = GetComponent<SizeController>();
        }

        private void Update() {
            _attackTimer.Update(Time.deltaTime);
            if (Input.GetMouseButton(0) && _attackTimer.IsFinished) {
                _attackTimer.Reset(attackTime);
                Attack();
            }
            float fAngle = Mathf.RoundToInt(Helpers.Instance.AngleToMouse(transform));
            if (fAngle < 0) { fAngle = 360 + fAngle; }
            int angle = (int)((fAngle + 45.0f) / 90.0f) * 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void Attack() {
            _animator.Play(_attack);
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, attackRange, _mask)) {
                if (!collider.TryGetComponent(out Health health) || Vector2.Dot((collider.transform.position - transform.position).normalized, transform.up) >= 0.5f) {
                    continue;
                }
                health.Damage(damage);
            }
        }

        public void DamageModifier(float magnitude) {
            _damageModifier *= magnitude;
        }
    }
}