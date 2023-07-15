using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Tower : MonoBehaviour
{
    public TowerStatsSO towerStatsSO;
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform towerWeaponHolder;
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
        if (enemyTarget )
        {
            RotateTowerWeaponToTarget();
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

    private void RotateTowerWeaponToTarget()
    {
        // Calculate the direction from the object's position to the target position
        Vector3 direction = enemyTarget.transform.position - towerWeaponHolder.position;


        // Calculate the angle in radians
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the object
        towerWeaponHolder.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

   public void DetectEnemies()
{
    results = Physics2D.OverlapCircleAll(transform.position + sensorOffset, sensorRadius, enemyLayer);

    if (results.Length > 0)
    {
        float closestDistance = float.MaxValue;
        Enemy closestEnemy = null;

        foreach (Collider2D enemyCollider in results)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();

            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        enemyTarget = closestEnemy;
    }
    else
    {
        enemyTarget = null; // No enemies detected, so clear the target
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
        return currentTowerLevel - 1;
    }
}
