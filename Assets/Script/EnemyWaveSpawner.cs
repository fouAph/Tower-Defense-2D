using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    public static EnemyWaveSpawner Singleton;
    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] Transform spawnPosition;
    public EnemyWaveLevelSO[] enemyWaveDatas;

    [SerializeField] float spawnRate = 1;
    [SerializeField] List<Enemy> enemyAlive;

    [SerializeField] List<Transform> wayPoints = new List<Transform>();

    [SerializeField] List<GameObject> enemyObjectsToSpawn = new List<GameObject>();

    private float lastSpawned;

    private void Update()
    {
        if (enemyObjectsToSpawn.Count > 0)
            if (Time.time - lastSpawned > 1f / spawnRate)
            {
                lastSpawned = Time.time;

                SpawnEnemy();
            }
    }

    private void SpawnEnemy()
    {
        var enemyObj = PoolSystem.Singleton.SpawnFromPool(enemyObjectsToSpawn[0], spawnPosition.position, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.SetWayPoints(wayPoints);

        enemyObjectsToSpawn.RemoveAt(0);


    }

}