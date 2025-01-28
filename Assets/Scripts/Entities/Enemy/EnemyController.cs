using UnityEngine;
using System;
using Utilities;

namespace Entity.Enemy {
    abstract public class EnemyController : MonoBehaviour {

        protected class EnemyAnimations {
            public int Idle;
            public int Attack;

            public EnemyAnimations(string name) {
                Idle = Animator.StringToHash($"{name}_Idle");
                Attack = Animator.StringToHash($"{name}_Attack");
            }
        }

        public enum EnemyState { Idle, Attack, Death }
        public enum EnemyType { Melee, Ranged }

        protected EnemyManager _manager;
        protected Animator _animator;
        protected Rigidbody2D _rb2D;

        [SerializeField] protected EnemyState _state;
        [SerializeField] protected float _speed = 4f;
        [SerializeField] protected float _attackTime = 1f;
        [SerializeField] protected float _attackRange = 2f;
        [SerializeField] protected float _aggroRange = 5f;
        [SerializeField] protected float _damage = 5f;
        [SerializeField] protected CountDownTimer _attackTimer = new CountDownTimer(0f);

        protected EnemyAnimations _animations;

        private void Start() {
            _manager = transform.parent.GetComponent<EnemyManager>();
            _animations = new EnemyAnimations(GetEnemyType().ToString());
            _animator = GetComponentInChildren<Animator>();
            _rb2D = GetComponent<Rigidbody2D>();
            _animator.Play(_animations.Idle);
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

        protected virtual void Chase() { }
        protected virtual void EnterChase() { }

        protected virtual void Attack() { }
        protected virtual void EnterAttack() { }
    }
}