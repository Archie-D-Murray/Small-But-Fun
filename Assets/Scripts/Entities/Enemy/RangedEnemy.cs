using UnityEngine;

using Projectiles;

namespace Entity.Enemy {
    public class RangedEnemy : EnemyController {

        [SerializeField] private GameObject _projectile;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectileDuration;
        [SerializeField] private float _runRange;

        public override EnemyType GetEnemyType() {
            return EnemyType.Melee;
        }

        protected override void EnterIdle() {
            _animator.Play(_animations.Idle);
        }

        protected override void Idle() {
            if (Vector2.Distance(_manager.PlayerPosition(), _rb2D.position) <= _attackRange) {
                SwitchState(EnemyState.Attack);
            }
        }

        protected override void Attack() {

            if (!_manager.InRange(_aggroRange, _rb2D.position)) {
                SwitchState(EnemyState.Idle);
                return;
            }


            if (_manager.InRange(_runRange, _rb2D.position)) { // Run towards
                _rb2D.velocity = _speed * Vector2.ClampMagnitude(_rb2D.position - _manager.PlayerPosition(), 1f);
            } else if (_manager.InRange(_attackRange, _rb2D.position) && _attackTimer.IsFinished) { // Shoot
                _attackTimer.Reset(_attackTime);
                GameObject projectile = Instantiate(_projectile, _rb2D.position, _rb2D.position.RotationTo(_manager.PlayerPosition()));
                projectile.GetOrAddComponent<LinearProjectileMover>().Init(_projectileSpeed);
                projectile.GetOrAddComponent<AutoDestroy>().Init(_projectileDuration);
                projectile.GetOrAddComponent<EntityDamager>().Init(_damage);
            } else { // Run away
                _rb2D.velocity = _speed * Vector2.ClampMagnitude(_manager.PlayerPosition() - _rb2D.position, 1f);
            }
        }
    }
}