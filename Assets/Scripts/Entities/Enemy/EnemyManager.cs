using UnityEngine;

namespace Entities.Enemy {
    public class EnemyManager : MonoBehaviour {
        [SerializeField] private Transform _player;

        private void Start() {
            _player = FindFirstObjectByType<Entities.Player.PlayerController>()?.transform ?? null;
        }

        public bool HasTarget() { return _player; }
        public Vector3 PlayerPosition() { return _player.position; }
    }
}