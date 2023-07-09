using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class Tower : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform projectilePrefab;
    [SerializeField] float projectileMoveSpeed = 5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float detectionRadius = 6f;
    [SerializeField] Enemy enemyTarget;

    [SerializeField] int fireRate = 2;
    [SerializeField] int weaponDamage = 5;
    [SerializeField] private const int sensorBuffer = 5;
    private Collider2D[] results = new Collider2D[sensorBuffer];
    private float lastFired;

    private int enemyCount;

    void Update()
    {
        DetectEnemies();
        if (enemyTarget && enemyCount > 0)
        {
            if (Time.time - lastFired > 1f / fireRate)
            {
                lastFired = Time.time;
                GameObject p = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity).gameObject;
                BulletProjectile bp = p.GetComponent<BulletProjectile>();
                // bp.SetDamage(weaponDamage);
                bp.SetupBullet(enemyTarget, projectileMoveSpeed, weaponDamage);
            }
        }
    }

    public void DetectEnemies()
    {
        // Collider2D[] 
        enemyCount = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, results, enemyLayer);

        if (enemyCount > 0)
        {
            enemyTarget = results[0].GetComponent<Enemy>();
        }
    }

    // private IEnumerator MoveProjectile(GameObject p, Vector3 targetPos)
    // {
    //     // Calculate the direction from the object's position to the target position
    //     Vector3 direction = targetPos - p.transform.position;

    //     // Calculate the angle in radians
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    //     // Set the rotation of the object
    //     p.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    //     while (true)
    //     {
    //         // Move the projectile towards the target position
    //         p.transform.position = Vector3.MoveTowards(p.transform.position, targetPos, Time.deltaTime * projectileSpeed);

    //         // Recalculate the distance between the projectile and the target position
    //         float dist = Vector3.Distance(p.transform.position, targetPos);

    //         if (dist <= 0.12f)
    //         {
    //             p.gameObject.SetActive(false);
    //             enemyTarget.OnDamaged(weaponDamage);
    //             print(weaponDamage); 
    //             print("hit");
    //             break; // Exit the while loop
    //         }

    //         yield return null;
    //     }
    // }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
