using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Enemy : MonoBehaviour, IDamageable, IPooledObject
{
    private const int WORLDBAR_SCALE = 8;
    [SerializeField] EnemyStatsSO enemyStatsSO;
    [SerializeField] List<Transform> currentWayPointList = new List<Transform>();
    private World_Bar healthBar;

    private float maxHealth = 100;
    private float moveSpeed = 5f;
    private int attackDamage = 2;
    private int currentWayPoint;

    private float currentHealth;

    private void Start()
    {
        healthBar = new World_Bar(transform, new Vector3(-0.600000024f, 0.699999988f, 0), Vector3.one, Color.white, Color.red, WORLDBAR_SCALE, 15);
        EnemySetup();
    }

    private void EnemySetup()
    {
        maxHealth = enemyStatsSO.health;
        moveSpeed = enemyStatsSO.moveSpeed;
        attackDamage = enemyStatsSO.attackDamage;

        currentHealth = maxHealth;

    }

    private void UpdateHealth()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        float healthBarSize = WORLDBAR_SCALE * healthPercentage;
        print(healthBarSize);
        healthBar.SetSize(healthBarSize);
    }

    private void Update()
    {
        MoveToWayPoint();
    }

    private void MoveToWayPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWayPointList[currentWayPoint].position,
                                moveSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, currentWayPointList[currentWayPoint].position);

        if (dist <= 0.01f)
        {
            currentWayPoint++;

            if (currentWayPoint >= currentWayPointList.Count)
            {
                gameObject.SetActive(false);
            }
        }


    }

    public List<Transform> GetWayPoints()
    {
        return currentWayPointList;
    }

    public void SetWayPoints(List<Transform> wp)
    {
        currentWayPointList.Clear();
        foreach (var item in wp)
        {
            currentWayPointList.Add(item);
        }
    }

    public void OnDamaged(int damage)
    {
        currentHealth -= damage;
        // health = Mathf.Clamp(health, health, 0);
        UpdateHealth();
        if (currentHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnObjectSpawn()
    {
        EnemySetup();
    }
}
