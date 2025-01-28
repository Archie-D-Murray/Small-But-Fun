using UnityEngine;

namespace Entities.Enemy {
    abstract public class EnemyController : MonoBehaviour {

        public enum EnemyState { Idle, Chase, Attack }
        private EnemyManager _manager;
        [SerializeField] private float _speed = 4f;

        private void Start() {
            _manager = transform.parent.GetComponent<EnemyManager>();
        }

        protected virtual void Idle() { }
        protected virtual void EnterIdle() { }

        protected virtual void Chase() { }
        protected virtual void EnterChase() { }

        protected virtual void Attack() { }
        protected virtual void EnterAttack() { }
    }
}