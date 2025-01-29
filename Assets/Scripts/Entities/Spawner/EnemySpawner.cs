using System.Collections.Generic;

using Entity.Enemy;

using UnityEngine;

using Utilities;

namespace Entity.Spawner {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private SpawnStrategy _strategy;
        [SerializeField] private CountDownTimer _spawnTimer = new CountDownTimer(0f);
        [SerializeField] private int _spawnCount = 0;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private EnemyManager _manager;
        [SerializeField] private int _spawnIndex;

        private void Start() {
            _spawnTimer.Reset(_strategy.InitialDelay);
            if (!_manager) {
                _manager = FindObjectOfType<EnemyManager>();
            }
        }

        public void FixedUpdate() {
            _spawnTimer.Update(Time.fixedDeltaTime);
            if (_strategy.CanSpawn(_spawnCount) && _spawnTimer.IsFinished) {
                _spawnCount += _strategy.Spawn(_strategy.GetSpawnPoint(_spawnPoints, ref _spawnIndex), _prefab, _manager);
                _spawnTimer.Reset(_strategy.SpawnDelay);
            }
        }
    }
}