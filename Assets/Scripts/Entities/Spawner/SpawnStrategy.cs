using System.Collections.Generic;

using Entity.Enemy;

using UnityEngine;

namespace Entity.Spawner {
    public abstract class SpawnStrategy : ScriptableObject {
        public float SpawnDelay = 3f;
        public int SpawnAmount = 3;
        public float InitialDelay = 0f;
        public abstract int Spawn(Vector3 position, GameObject prefab, EnemyManager manager);
        public abstract bool CanSpawn(int spawnCount);
        public abstract Vector3 GetSpawnPoint(Transform[] spawnPoints, ref int spawnIndex);
    }
}