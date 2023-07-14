using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Tower : MonoBehaviour
{
    public TowerStatsSO towerStatsSO;
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform projectilePrefab;
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] int currentTowerLevel = 1;

    [SerializeField] float sensorRadius = 20f;
    [SerializeField] Vector3 sensorOffset;

    [SerializeField] float fireRate = 2;
    [SerializeField] int weaponDamage = 5;
    [SerializeField] float projectileMoveSpeed = 5f;

    private const int sensorBuffer = 5;
    private SpriteRenderer towerSpriteRenderer;
    private Collider2D[] results = new Collider2D[sensorBuffer];
    private Enemy enemyTarget;
    private float lastFired;

    private int enemyCount;

    private void Start()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(projectilePrefab.gameObject, 50);
        towerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        towerStatsSO.SetTowerSprite(towerSpriteRenderer, currentTowerLevel);
    }

    private void StatSetup()
    {
        fireRate = towerStatsSO.fireRate;
        sensorRadius = towerStatsSO.sensorRadius;
    }

    void Update()
    {
        DetectEnemies();
        if (enemyTarget && enemyCount > 0)
        {
            if (Time.time - lastFired > 1f / fireRate)
            {
                lastFired = Time.time;
                FireBullet();
            }
        }
    }

    private void FireBullet()
    {
        GameObject p = PoolSystem.Singleton.SpawnFromPool(projectilePrefab.gameObject, shootPoint.position, Quaternion.identity);
        BulletProjectile bp = p.GetComponent<BulletProjectile>();
        bp.SetupBullet(enemyTarget, projectileMoveSpeed, weaponDamage);
    }

    public void DetectEnemies()
    {
        // Collider2D[] 
        enemyCount = Physics2D.OverlapCircleNonAlloc(transform.position + sensorOffset, sensorRadius, results, enemyLayer);

        if (enemyCount > 0)
        {
            enemyTarget = results[0].GetComponent<Enemy>();
        }
    }

    public float GetSensorRadius()
    {
        return sensorRadius;
    }

    private void OnMouseDown()
    {
        TowerUpgrade.Singleton.ShowSensorOverlay(this);
    }

    /*private void OnMouseEnter()
     {
         TowerUpgrade.Singleton.ShowSensorOverlay(this);
     }*/

    /*  private void OnMouseExit()
     {
         TowerUpgrade.Singleton.HideSensorOverlay();
     }*/

    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + sensorOffset, sensorRadius);
    }

    public Vector3 GetSensorOffset()
    {
        return sensorOffset;
    }

    public void UpgradeTower()
    {
        currentTowerLevel++;
        fireRate += towerStatsSO.towerStatsUpgrades[currentTowerLevel - 1].fireRateUpgrade;
        sensorRadius += towerStatsSO.towerStatsUpgrades[currentTowerLevel - 1].sensorRadiusUpgrade;
        weaponDamage += towerStatsSO.towerStatsUpgrades[currentTowerLevel - 1].weaponDamageUpgrade;
        towerStatsSO.SetTowerSprite(towerSpriteRenderer, currentTowerLevel);

    }

    public bool CanBuyTower()
    {
        return towerStatsSO.CanBuyTower(GameManager.Singleton.GetCoin());
    }

    public bool CanUpgradeTower()
    {
        return towerStatsSO.CanUpgradeTower(GameManager.Singleton.GetCoin(), currentTowerLevel);
    }

    public bool CheckIfMaxUpgrade()
    {
        return currentTowerLevel == towerStatsSO.towerStatsUpgrades.Length;
    }

    public int GetCurrentTowerLevel()
    {
        return currentTowerLevel-1;
    }
}
