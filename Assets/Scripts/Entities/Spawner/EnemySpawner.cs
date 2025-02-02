using System;
using System.Collections.Generic;

using Entity.Enemy;

using UnityEngine;

using Utilities;

using Tags;

namespace Entity.Spawner {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private SpawnStrategy _strategy;
        [SerializeField] private CountDownTimer _spawnTimer = new CountDownTimer(0f);
        [SerializeField] private int _spawnCount = 0;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private EnemyManager _manager;
        [SerializeField] private int _spawnIndex;
        [SerializeField] private bool _allowSpawning = true;
        [SerializeField] private Room _room = null;

        public Action<EnemySpawner> OnFinish;

        public bool IsDone => !_strategy.CanSpawn(_spawnCount);

        private void Start() {
            if (transform.parent.OrNull()?.TryGetComponent(out Room room) ?? false) {
                room.RegisterSpawner(this);
                _allowSpawning = false;
                _room = room;
                SpawnPoint spawnPoints = _room.GetComponentInChildren<SpawnPoint>();
                if (spawnPoints) {
                    _spawnPoints = new Transform[spawnPoints.transform.childCount];
                    for (int i = 0; i < _spawnPoints.Length; i++) {
                        _spawnPoints[i] = spawnPoints.transform.GetChild(i);
                    }
                }
            }
            _spawnTimer.Reset(_strategy.InitialDelay);
            if (!_manager) {
                _manager = FindObjectOfType<EnemyManager>();
            }
        }

        private void FixedUpdate() {
            _spawnTimer.Update(Time.fixedDeltaTime);
            if (_spawnTimer.IsFinished && _allowSpawning) {
                _spawnCount += _strategy.Spawn(_strategy.GetSpawnPoint(_spawnPoints, ref _spawnIndex), _prefab, _manager, _room);
                _spawnTimer.Reset(_strategy.SpawnDelay);
                if (!_strategy.CanSpawn(_spawnCount)) {
                    enabled = false; // Done spawning
                    _allowSpawning = false;
                }
            }
        }

        public void EnableSpawning() {
            _allowSpawning = true;
        }
    }
}