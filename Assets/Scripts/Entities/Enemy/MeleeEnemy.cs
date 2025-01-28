using UnityEngine;


namespace Entity.Enemy {
    public class MeleeEnemy : EnemyController {
        public override EnemyType GetEnemyType() {
            return EnemyType.Melee;
        }

        protected override void EnterIdle() {
            _animator.Play(_animations.Idle);
        }

        protected override void Idle() {
            if (Vector2.Distance(_manager.PlayerPosition(), transform.position) <= _attackRange) {
                SwitchState(EnemyState.Attack);
            }
        }

        protected override void Attack() {

            if (!_manager.InRange(_aggroRange, transform.position)) {
                SwitchState(EnemyState.Idle);
                return;
            }

            _rb2D.velocity = _speed * Vector2.ClampMagnitude(_manager.PlayerPosition() - _rb2D.position, 1f);

            if (_manager.InRange(_attackRange, transform.position) && _attackTimer.IsFinished) {
                _attackTimer.Reset(_attackTime);
                foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, _attackRange, _manager.PlayerLayer)) {
                    if (!collider.TryGetComponent(out Health health)) { continue; }
                    health.Damage(_damage);
                }
            }
        }
    }
}