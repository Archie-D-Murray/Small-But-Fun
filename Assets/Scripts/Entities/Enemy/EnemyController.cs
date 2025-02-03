using UnityEngine;
using System;
using Utilities;
using Projectiles;
using Rooms;
using Audio;

namespace Entity.Enemy {
    [RequireComponent(typeof(SFXEmitter))]
    abstract public class EnemyController : MonoBehaviour {

        protected class EnemyAnimations {
            public static int Idle = Animator.StringToHash("Idle");
            public static int Attack = Animator.StringToHash("Attack");
            public static int Speed = Animator.StringToHash("Speed");
        }

        public enum EnemyState { Idle, Attack, Death }
        public enum EnemyType { Melee, Ranged, Turret }

        protected EnemyManager _manager;
        protected Animator _animator;
        protected Rigidbody2D _rb2D;
        protected Health _health;
        protected SpriteRenderer _spriteRenderer;
        protected Material _normalMaterial;
        protected SFXEmitter _emitter;

        [SerializeField] protected EnemyState _state;
        [SerializeField] protected float _speed = 4f;
        [SerializeField] protected float _attackTime = 1f;
        [SerializeField] protected float _attackRange = 2f;
        [SerializeField] protected float _aggroRange = 5f;
        [SerializeField] protected float _damage = 5f;
        [SerializeField] protected CountDownTimer _attackTimer = new CountDownTimer(0f);

        private void Awake() {
            _health = GetComponent<Health>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _normalMaterial = _spriteRenderer.material;
            _health.OnDeath += () => SwitchState(EnemyState.Death);
            _health.OnDamage += (float _) => {
                _spriteRenderer.FlashDamage(AssetServer.Instance.FlashMaterial, _normalMaterial, 0.25f, this);
                _emitter.Play(SoundEffectType.Hit);
            };
            _animator = GetComponentInChildren<Animator>();
            _emitter = GetComponent<SFXEmitter>();
            _rb2D = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start() {
            _manager = transform.parent.GetComponent<EnemyManager>();
            _animator.Play(EnemyAnimations.Idle);
        }

        public void RoomSpawn(Room room) {
            room.OnEnemySpawn(this);
            if (!_health) {
                _health = GetComponent<Health>();
            }
            _health.OnDeath += () => {
                room.OnEnemyDeath(this);
                Instantiate(AssetServer.Instance.EnemyDeath, transform.position, Quaternion.identity).AddComponent<AutoDestroy>().Init(1f);
                _emitter.Play(SoundEffectType.Death);
            };
        }

        public abstract EnemyType GetEnemyType();

        private void Update() {
            if (_state == EnemyState.Death) {
                return;
            }

            UpdateTimers();

            if (!_manager.HasTarget()) {
                SwitchState(EnemyState.Idle);
            }

            switch (_state) {
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Death:
                    break;
                default:
                    Debug.Log($"AHHHHHHHH, What is state: {_state}");
                    break;
            }
        }

        protected void SwitchState(EnemyState newState) {
            if (_state == newState) {
                return;
            }
            switch (newState) {
                case EnemyState.Idle:
                    EnterIdle();
                    _state = newState;
                    break;
                case EnemyState.Attack:
                    EnterAttack();
                    _state = newState;
                    break;
                case EnemyState.Death:
                    _state = newState;
                    EnterDeath();
                    break;
                default:
                    Debug.Log($"AHHHHHHHH, What is state: {_state}");
                    break;
            }
        }

        protected virtual void UpdateTimers() {
            _attackTimer.Update(Time.deltaTime);
        }

        protected virtual void Idle() { }
        protected virtual void EnterIdle() { }

        protected virtual void Attack() { }
        protected virtual void EnterAttack() { }

        protected virtual void EnterDeath() { }
    }
}