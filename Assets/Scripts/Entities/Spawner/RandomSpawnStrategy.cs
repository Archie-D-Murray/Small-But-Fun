using Entity.Enemy;

using UnityEngine;

namespace Entity.Spawner {
    [CreateAssetMenu(menuName = "Spawner/Random", fileName = "Random Settings")]
    public sealed class RandomSpawnStrategy : SpawnStrategy {
        public override bool CanSpawn(int spawnCount) {
            return true;
        }

        public override Vector3 GetSpawnPoint(Transform[] spawnPoints, ref int spawnIndex) {
            spawnIndex = ++spawnIndex % spawnPoints.Length;
            return spawnPoints[UnityEngine.Random.Range(spawnIndex, spawnPoints.Length)].position;
        }

        public override int Spawn(Vector3 position, GameObject prefab, EnemyManager manager, Room room = null) {
            GameObject spawned = Instantiate(prefab, manager.transform);
            spawned.transform.SetPositionAndRotation(position, Quaternion.identity);
            RoomSpawnCallback(room, spawned);
            return 1;
        }
    }
}