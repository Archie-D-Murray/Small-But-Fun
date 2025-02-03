using UnityEngine;

using Projectiles;
using Audio;

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
            _rb2D.velocity = Vector2.zero;
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

            _animator.SetFloat(EnemyAnimations.Speed, _rb2D.velocity.sqrMagnitude / _speed * _speed);

            transform.rotation = _rb2D.position.RotationCardinalTo(_manager.PlayerPosition());
            if (_manager.InRange(_runRange, _rb2D.position)) { // Run towards
                _rb2D.velocity = _speed * Vector2.ClampMagnitude(_rb2D.position - _manager.PlayerPosition(), 1f);
            } else if (_manager.InRange(_attackRange, _rb2D.position) && _attackTimer.IsFinished) { // Shoot
                _emitter.Play(SoundEffectType.Attack);
                _attackTimer.Reset(_attackTime);
                GameObject projectile = Instantiate(_projectile, _rb2D.position, _rb2D.position.RotationTo(_manager.PlayerPosition()));
                projectile.GetOrAddComponent<LinearProjectileMover>().Init(_projectileSpeed);
                projectile.GetOrAddComponent<AutoDestroy>().Init(_projectileDuration);
                projectile.GetOrAddComponent<EntityDamager>().Init(_damage);
                _animator.Play(EnemyAnimations.Attack);
            } else { // Run away
                _rb2D.velocity = _speed * Vector2.ClampMagnitude(_manager.PlayerPosition() - _rb2D.position, 1f);
            }
        }

        protected override void EnterDeath() {
            _rb2D.velocity = Vector2.zero;
            Destroy(gameObject, 0.1f);
        }
    }
}