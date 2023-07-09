using System.Collections;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Enemy enemyTarget;
    private Vector3 targetPos;
    private float moveSpeed;
    private int weaponDamage;

    private void Update()
    {
        MoveProjectile(enemyTarget, moveSpeed, weaponDamage);
    }

    public void SetupBullet(Enemy _enemyTarget, float _moveSpeed, int _weaponDamage)
    {
        enemyTarget = _enemyTarget;
        targetPos = enemyTarget.transform.position;
        moveSpeed = _moveSpeed;
        weaponDamage = _weaponDamage;
    }
    private void MoveProjectile(Enemy enemyTransform, float moveSpeed, int weaponDamage)
    {
        // Calculate the direction from the object's position to the target position
        Vector3 direction = targetPos - transform.position;


        // Calculate the angle in radians
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the object
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // Move the projectile towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);

        // Recalculate the distance between the projectile and the target position
        float dist = Vector3.Distance(transform.position, targetPos);

        if (dist <= 0.12f)
        {
            enemyTarget.OnDamaged(weaponDamage);
            gameObject.SetActive(false);
        }

    }

    private int damage;
    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     print("hit " + other.collider.name);
    //     IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
    //     if (damageable != null)
    //     {
    //         damageable.OnDamaged(damage);
    //         print("hit " + other.collider.name);
    //     }

    // }

    public void SetDamage(int value)
    {
        damage = value;
    }
}
