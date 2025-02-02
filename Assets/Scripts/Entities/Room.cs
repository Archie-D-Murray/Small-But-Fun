using UnityEngine;

using System.Collections.Generic;

using Entity.Enemy;
using Entity.Spawner;
using System;
using Entity.Player;
using Loot;
using Tags;

namespace Entity {
    public class Room : MonoBehaviour {
        [SerializeField] private List<EnemyController> _spawnedEnemies = new List<EnemyController>();
        [SerializeField] private List<EnemySpawner> _spawners = new List<EnemySpawner>();
        [SerializeField] private SizeController _sizeController;
        [SerializeField] private Wall _barrier;
        [SerializeField] private LootAnimator _lootAnimator;

        private void Start() {
            _lootAnimator = GetComponentInChildren<LootAnimator>(true);
            _barrier = GetComponentInChildren<Wall>(true);
        }

        public void RegisterSpawner(EnemySpawner spawner) {
            _spawners.Add(spawner);
            spawner.OnFinish += UnRegisterSpawner;
        }

        private void UnRegisterSpawner(EnemySpawner spawner) {
            _spawners.Remove(spawner);
            if (_spawners.Count == 0) {
                OnPlayerFinish();
            }
        }
        private void OnPlayerEnter() {
            foreach (EnemySpawner spawner in _spawners) {
                spawner.EnableSpawning();
            }
            _sizeController.ToggleSizeLock(gameObject, true);
            _barrier.gameObject.SetActive(true);
        }

        public void OnEnemyDeath(EnemyController enemy) {
            _spawnedEnemies.Remove(enemy);
            if (_spawnedEnemies.Count == 0) {
                OnPlayerFinish();
            }
        }

        public void OnEnemySpawn(EnemyController enemy) {
            _spawnedEnemies.Add(enemy);
        }

        private void OnPlayerFinish() {
            _sizeController.ToggleSizeLock(gameObject, false);
            _lootAnimator.gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D player) {
            if (player.TryGetComponent(out SizeController sizeController)) {
                _sizeController = sizeController;
                OnPlayerEnter();
            }
        }
    }
}