using UnityEngine;

using Utilities;

public class AssetServer : Singleton<AssetServer> {
    public GameObject enemyDeath;
    public GameObject smoke;
    public GameObject[] weaponDrops;
    public GameObject healthPack;
    public GameObject playerDeath;
    public GameObject playerHit;
    public Material flashMaterial;
    public LayerMask explosionMask;
    public LayerMask enemyMask;
}