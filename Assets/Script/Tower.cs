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

    [SerializeField] AudioClip shootSFX;

    [SerializeField] float sensorRadius = 20f;
    [SerializeField] Vector3 sensorOffset;

    [SerializeField] float projectileMoveSpeed = 50;
    // [SerializeField] float fireRate = 2;
    private float shootingSpeed;
    private int weaponDamage;

    private TowerWeapon towerWeapon;
    private SpriteRenderer towerSpriteRenderer;
    private Collider2D[] results;
    private Enemy enemyTarget;
    private float lastFired;

    private int currentTowerLevel;
    private int enemyCount;

    private void Start()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(projectilePrefab.gameObject, 50);
        towerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        towerWeapon = GetComponentInChildren<TowerWeapon>();
        // towerStatsSO.SetTowerSprite(towerSpriteRenderer, currentTowerLevel);
        StatSetup();

        towerWeapon.SetTower(this);
    }

    private void StatSetup()
    {
        // fireRate = towerStatsSO.fireRate;
        weaponDamage = towerStatsSO.baseWeaponDamage;
        shootingSpeed = towerStatsSO.baseShootingSpeed;
        sensorRadius = towerStatsSO.baseSensorRadius;
    }

    void Update()
    {
        DetectEnemies();

        //Set animator's IsShooting parameter 
        towerWeapon.SetShoot();
        if (enemyTarget)
        {
            RotateTowerWeaponToTarget();
        }
    }

    public void FireBullet()
    {
        GameObject p = PoolSystem.Singleton.SpawnFromPool(projectilePrefab.gameObject, shootPoint.position, Quaternion.identity);
        BulletProjectile bp = p.GetComponent<BulletProjectile>();
        bp.SetupBullet(enemyTarget, projectileMoveSpeed, weaponDamage);

        SoundManager.Singleton.PlayAudio(shootSFX);
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
        shootingSpeed += towerStatsSO.towerStatsUpgrades[currentTowerLevel].shootingSpeedUpgrade;
        towerWeapon.SetAnimationShootingSpeed(shootingSpeed);
        // fireRate += towerStatsSO.towerStatsUpgrades[currentTowerLevel ].fireRateUpgrade;
        sensorRadius += towerStatsSO.towerStatsUpgrades[currentTowerLevel].sensorRadiusUpgrade;
        weaponDamage += towerStatsSO.towerStatsUpgrades[currentTowerLevel].weaponDamageUpgrade;
        towerStatsSO.SetTowerSprite(towerSpriteRenderer, currentTowerLevel);
        currentTowerLevel++;

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
        return currentTowerLevel;
    }

    public bool CheckIsEnemyTargetAvailable()
    {
        return enemyTarget;
    }
}
