using UnityEngine;

using System.Collections.Generic;

using Entity.Enemy;
using Entity.Spawner;
using System;
using Entity.Player;
using Loot;
using Tags;
using UI;

namespace Rooms {
    public class Room : MonoBehaviour {
        [SerializeField] private List<EnemyController> _spawnedEnemies = new List<EnemyController>();
        [SerializeField] private List<EnemySpawner> _spawners = new List<EnemySpawner>();
        [SerializeField] private SizeController _sizeController;
        [SerializeField] private Wall _barrier;
        [SerializeField] private LootAnimator _lootAnimator;
        [SerializeField] private int _enemiesCleared = 0;
        [SerializeField] private int _roomEnemies = 0;

        private void Start() {
            _lootAnimator = GetComponentInChildren<LootAnimator>(true);
            _barrier = GetComponentInChildren<Wall>(true);
        }

        public void RegisterSpawner(EnemySpawner spawner) {
            _spawners.Add(spawner);
            spawner.OnFinish += UnRegisterSpawner;
            _roomEnemies += spawner.MaxSpawns;
        }

        private void UnRegisterSpawner(EnemySpawner spawner) {
            _spawners.Remove(spawner);
        }
        private void OnPlayerEnter() {
            foreach (EnemySpawner spawner in _spawners) {
                spawner.EnableSpawning();
            }
            _sizeController.ToggleSizeLock(gameObject, true);
            _barrier.gameObject.SetActive(true);
            PlayerUI.Instance.ToggleRoomReadout(true);
            GetComponent<Collider2D>().enabled = false;
            PlayerUI.Instance.RoomReadout.text = $"{_enemiesCleared} / {_roomEnemies} ({((float)_enemiesCleared / (float)_roomEnemies):0%})";
        }

        public void OnEnemyDeath(EnemyController enemy) {
            _spawnedEnemies.Remove(enemy);
            _enemiesCleared++;
            if (_spawnedEnemies.Count == 0 && _spawners.Count == 0) {
                OnPlayerFinish();
            }
            PlayerUI.Instance.RoomReadout.text = $"{_enemiesCleared} / {_roomEnemies} ({((float)_enemiesCleared / (float)_roomEnemies):0%})";
            if (_sizeController) { return; }
        }

        public void OnEnemySpawn(EnemyController enemy) {
            _spawnedEnemies.Add(enemy);
        }

        private void OnPlayerFinish() {
            _sizeController.ToggleSizeLock(gameObject, false);
            _lootAnimator.gameObject.SetActive(true);
            _barrier.gameObject.SetActive(false);
            PlayerUI.Instance.ToggleRoomReadout(false);
            RoomManager.Instance.RoomCleared();
        }

        private void OnTriggerEnter2D(Collider2D player) {
            if (RoomManager.Instance.CurrentRoom != this) {
                return;
            }
            if (player.TryGetComponent(out SizeController sizeController)) {
                _sizeController = sizeController;
                OnPlayerEnter();
            }
        }
    }
}