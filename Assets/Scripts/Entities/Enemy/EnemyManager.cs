using UnityEngine;

namespace Entity.Enemy {
    public class EnemyManager : MonoBehaviour {
        [SerializeField] private Transform _player;

        private void Start() {
            _player = FindFirstObjectByType<Entity.Player.PlayerController>()?.transform ?? null;
        }

        public bool HasTarget() { return _player; }
        public Vector2 PlayerPosition() { return _player.position; }
        public bool InRange(float range, Vector2 position) {
            if (!HasTarget()) { return false; }
            return Vector2.Distance(PlayerPosition(), position) <= range;
        }

        public LayerMask PlayerLayer;
    }
}