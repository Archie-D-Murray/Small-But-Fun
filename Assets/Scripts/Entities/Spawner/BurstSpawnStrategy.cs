using System.Collections.Generic;

using Entity.Enemy;

using UnityEngine;

namespace Entity.Spawner {
    [CreateAssetMenu(menuName = "Spawner/Burst", fileName = "Burst Settings")]
    public sealed class BurstSpawnStrategy : SpawnStrategy {
        public int BurstCount = 6;
        public override bool CanSpawn(int spawnCount) {
            return spawnCount < BurstCount;
        }

        public override Vector3 GetSpawnPoint(Transform[] spawnPoints, ref int spawnIndex) {
            spawnIndex = ++spawnIndex % spawnPoints.Length;
            return spawnPoints[spawnIndex].position;
        }

        public override int Spawn(Vector3 position, GameObject prefab, EnemyManager manager, Room room = null) {
            for (int i = 0; i < BurstCount; i++) {
                GameObject spawned = Instantiate(prefab, manager.transform);
                spawned.transform.SetPositionAndRotation(position + (Vector3)Random.insideUnitCircle, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                RoomSpawnCallback(room, spawned);
            }

            return BurstCount;
        }
    }
}