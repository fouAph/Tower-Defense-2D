using System;
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
    [SerializeField] float spawnRate = 1;
    [SerializeField] List<Transform> wayPoints = new List<Transform>();
    [SerializeField] List<Enemy> enemyToSpawn = new List<Enemy>();

    // public event EventHandler onEnemySpawn;

    private float lastSpawned;

    private void Update()
    {
        if (GameManager.Singleton.gameState != GameState.InGame) return;

        if (enemyToSpawn.Count > 0)
            if (Time.time - lastSpawned > 1f / spawnRate)
            {
                lastSpawned = Time.time;

                SpawnEnemy();
            }
    }

    private void SpawnEnemy()
    {
        var enemyObj = PoolSystem.Singleton.SpawnFromPool(enemyToSpawn[0].gameObject, spawnPosition.position, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.SetWayPoints(wayPoints);

        enemyToSpawn.RemoveAt(0); 
    }
  
    public void AddEnemyToSpawnList(List<Enemy> enemyList)
    {
        foreach (var item in enemyList)
        {
            enemyToSpawn.Add(item);
        }
    }

}