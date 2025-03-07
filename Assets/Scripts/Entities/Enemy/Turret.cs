using Audio;

using Projectiles;

using UnityEngine;

using Utilities;

namespace Entity.Enemy {
    public class Turret : EnemyController {
        [SerializeField] private GameObject _projectile;
        private CountDownTimer _idleLookTimer = new CountDownTimer(2f);
        [SerializeField] private int lookDir;
        [SerializeField] private float _projectileSpeed = 2f;
        [SerializeField] private float _projectileDuration = 3f;

        public override EnemyType GetEnemyType() {
            return EnemyType.Turret;
        }

        protected override void UpdateTimers() {
            base.UpdateTimers();
            _idleLookTimer.Update(Time.fixedDeltaTime);
        }

        protected override void EnterIdle() {
            _animator.Play(EnemyAnimations.Idle);
        }

        protected override void Idle() {
            if (!_manager.HasTarget()) {
                return;
            }
            if (_manager.InRange(_attackRange, _rb2D.position)) {
                SwitchState(EnemyState.Attack);
                return;
            }
            if (_idleLookTimer.IsFinished) {
                _idleLookTimer.Reset();
                _idleLookTimer.Start();
                lookDir = (lookDir + 90) % 360;
                transform.rotation = Quaternion.AngleAxis(lookDir, Vector3.forward);
            }
        }

        protected override void Attack() {
            transform.rotation = _rb2D.position.RotationCardinalTo(_manager.PlayerPosition());
            if (!_manager.InRange(_attackRange, _rb2D.position)) {
                SwitchState(EnemyState.Idle);
                return;
            }

            if (_attackTimer.IsFinished) {
                _emitter.Play(SoundEffectType.Attack);
                _animator.Play(EnemyAnimations.Attack);
                _attackTimer.Reset(_attackTime);
            }
        }

        public override void AttackFrame() {
            GameObject projectile = Instantiate(_projectile, _rb2D.position, _rb2D.position.RotationTo(_manager.PlayerPosition()));
            projectile.GetOrAddComponent<LinearProjectileMover>().Init(_projectileSpeed);
            projectile.GetOrAddComponent<AutoDestroy>().Init(_projectileDuration);
            projectile.GetOrAddComponent<EntityDamager>().Init(_damage);
        }
        protected override void EnterDeath() {
            Destroy(gameObject, 0.1f);
        }
    }
}