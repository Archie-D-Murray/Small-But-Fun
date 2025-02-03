using System.Collections.Generic;

using Entity.Enemy;

using UnityEngine;

using Rooms;

namespace Entity.Spawner {
    public abstract class SpawnStrategy : ScriptableObject {
        public float SpawnDelay = 3f;
        public int SpawnAmount = 3;
        public float InitialDelay = 0f;
        public abstract int Spawn(Vector3 position, GameObject prefab, EnemyManager manager, Room room = null);
        public abstract bool CanSpawn(int spawnCount);
        public abstract Vector3 GetSpawnPoint(Transform[] spawnPoints, ref int spawnIndex);

        protected void RoomSpawnCallback(Room room, GameObject spawned) {
            if (room) {
                spawned.GetComponent<EnemyController>().RoomSpawn(room);
            }
        }
    }
}