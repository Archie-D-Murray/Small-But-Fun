using Audio;

using UnityEngine;


namespace Entity.Enemy {
    public class MeleeEnemy : EnemyController {

        protected override void Start() {
            base.Start();
            _health.OnDamage += (float _) => Debug.Log($"{name} got hit");
        }

        public override EnemyType GetEnemyType() {
            return EnemyType.Melee;
        }

        protected override void EnterIdle() {
            _animator.Play(EnemyAnimations.Idle);
        }

        protected override void Idle() {
            if (_manager.InRange(_aggroRange, transform.position)) {
                SwitchState(EnemyState.Attack);
            }
        }

        protected override void Attack() {
            _animator.SetFloat(EnemyAnimations.Speed, _rb2D.velocity.sqrMagnitude / (_speed * _speed));
            if (!_manager.InRange(_aggroRange, transform.position)) {
                SwitchState(EnemyState.Idle);
                return;
            }

            _rb2D.velocity = _speed * Vector2.ClampMagnitude(_manager.PlayerPosition() - _rb2D.position, 1f);

            if (_manager.InRange(_attackRange, transform.position) && _attackTimer.IsFinished) {
                _emitter.Play(SoundEffectType.Attack);
                _attackTimer.Reset(_attackTime);
                _animator.Play(EnemyAnimations.Attack);
            }
        }

        protected override void EnterDeath() {
            _rb2D.velocity = Vector2.zero;
            Destroy(gameObject, 0.1f);
        }

        public override void AttackFrame() {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, _attackRange, _manager.PlayerLayer)) {
                if (!collider.TryGetComponent(out Health health)) { continue; }
                health.Damage(_damage);
            }
        }
    }
}