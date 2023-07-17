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
    [SerializeField] float spawnRate = 1;   //1 enemy persecond
    [SerializeField] EnemyPath[] enemyPaths;
    private List<Enemy> enemyToSpawn = new List<Enemy>();

    int rand;
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
        rand = UnityEngine.Random.Range(0, enemyPaths.Length);
        var enemyObj = PoolSystem.Singleton.SpawnFromPool(enemyToSpawn[0].gameObject, enemyPaths[rand].spawnPoint.position, Quaternion.identity);

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        List<Transform> wp = new List<Transform>();
        foreach (var item in enemyPaths[rand].wayPoints)
        {
            wp.Add(item);
        }
        enemy.SetWayPoints(wp);

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
[System.Serializable]
public struct EnemyPath
{
    public Transform spawnPoint;
    public Transform[] wayPoints;
}