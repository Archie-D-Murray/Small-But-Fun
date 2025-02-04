using System;
using System.Collections.Generic;

using Entity.Enemy;

using UnityEngine;

using Utilities;

using Tags;
using Rooms;

namespace Entity.Spawner {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private SpawnStrategy _strategy;
        [SerializeField] private CountDownTimer _spawnTimer;
        [SerializeField] private int _spawnCount = 0;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private EnemyManager _manager;
        [SerializeField] private int _spawnIndex;
        [SerializeField] private bool _allowSpawning = true;
        [SerializeField] private Room _room = null;

        public Action<EnemySpawner> OnFinish;

        public bool IsDone => !_strategy.CanSpawn(_spawnCount);
        public int MaxSpawns => _strategy.SpawnAmount;

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
            _spawnTimer = new CountDownTimer(_strategy.InitialDelay);
            _spawnTimer.Start();
            if (!_manager) {
                _manager = FindObjectOfType<EnemyManager>();
            }
        }

        private void FixedUpdate() {
            _spawnTimer.Update(Time.fixedDeltaTime);
            if (_spawnTimer.IsFinished && _allowSpawning) {
                _spawnCount += _strategy.Spawn(_strategy.GetSpawnPoint(_spawnPoints, ref _spawnIndex), _prefab, _manager, _room);
                Debug.Log($"Spawner {name} spawned");
                _spawnTimer.Reset(_strategy.SpawnDelay);
                if (!_strategy.CanSpawn(_spawnCount)) {
                    enabled = false; // Done spawning
                    _allowSpawning = false;
                    OnFinish?.Invoke(this);
                    Debug.Log($"Spawner {name} finished");
                }
            }
        }

        public void EnableSpawning() {
            _allowSpawning = true;
        }
    }
}