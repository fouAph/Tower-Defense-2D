using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] int health = 100;
    [SerializeField] float moveSpeed = 5f;
    private World_Bar healthBar;
    [SerializeField] List<Transform> wayPoints = new List<Transform>();
    private int currentWayPoint;

    private void Start()
    {
        healthBar = new World_Bar(transform, Vector3.up, Vector3.one, Color.white, Color.red, health, 15);
        // healthBar.
    }

    public void UpdateHealth()
    {
        healthBar.SetSize(health);
    }

    private void Update()
    {
        MoveToWayPoint();
    }

    private void MoveToWayPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentWayPoint].position,
                                moveSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, wayPoints[currentWayPoint].position);

        if (dist <= 0.01f)
        {
            currentWayPoint++;

            if (currentWayPoint >= wayPoints.Count)
            {
                gameObject.SetActive(false);
            }
        }


    }


    public void OnDamaged(int damage)
    {
        health -= damage;
        // health = Mathf.Clamp(health, health, 0);

        if (health == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
