using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Enemy : MonoBehaviour, IDamageable, IPooledObject
{
    private const int WORLDBAR_SCALE = 9;
    [SerializeField] EnemyStatsSO enemyStatsSO;
    [SerializeField] GameObject hitVFXPrefab;
    [SerializeField] List<Transform> currentWayPointList = new List<Transform>();

    [SerializeField] bool flipWhenTurnLeft;
    [SerializeField] bool flipWhenTurnRight;

    private World_Bar healthBar;

    private float maxHealth = 100;
    private float moveSpeed = 5f;
    private int attackDamage = 2;
    private int currentWayPoint;
    private Animator animator;
    private float currentHealth;

    private int AnimatorHashToIntMoveLeft;
    private int AnimatorHashToIntMoveRight;
    private int AnimatorHashToIntMoveUp;
    private int AnimatorHashToIntMoveDown;
    private SpriteRenderer spriteRenderer;

    private bool isDead;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        healthBar = new World_Bar(transform, new Vector3(-0.219999999f, 0.300000012f, 0), Vector3.one, Color.white, Color.red, WORLDBAR_SCALE, 50);

        AnimatorHashToIntMoveUp = Animator.StringToHash("MoveUp");
        AnimatorHashToIntMoveDown = Animator.StringToHash("MoveDown");
        AnimatorHashToIntMoveLeft = Animator.StringToHash("MoveLeft");
        AnimatorHashToIntMoveRight = Animator.StringToHash("MoveRight");

        EnemySetup();
        PoolSystem.Singleton.AddObjectToPooledObject(hitVFXPrefab, 15);

        PlayAnimationBasedOnTargetDirection(currentWayPointList[currentWayPoint]);
    }

    private void Update()
    {
        if (GameManager.Singleton.gameState != GameState.InGame) return;
        MoveToWayPoint();
    }

    private void EnemySetup()
    {
        isDead = false;
        maxHealth = enemyStatsSO.maxHealth;
        moveSpeed = enemyStatsSO.moveSpeed;
        attackDamage = enemyStatsSO.attackDamage;

        currentHealth = maxHealth;
        currentWayPoint = 0;
    }

    private void PlayAnimation(int stringHash)
    {
        animator.Play(stringHash);
    }

    private void MoveToWayPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWayPointList[currentWayPoint].position,
                                moveSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, currentWayPointList[currentWayPoint].position);

        if (dist <= 0.01f)
        {

            if (currentWayPoint < currentWayPointList.Count)
            {
                currentWayPoint++;
                if (currentWayPoint == currentWayPointList.Count)
                {
                    //TODO When Arrived, Damage The Player (Decrease player Health)
                    GameManager.Singleton.OnDamaged(enemyStatsSO.attackDamage);
                    RemoveThisEnemyFromEnemyAlive();

                    if (GameManager.Singleton.CheckIfNoEnemyLeft())
                        GameManager.Singleton.OnWaveCompleted_Invoke();

                    gameObject.SetActive(false);
                    return;
                }
            }

            PlayAnimationBasedOnTargetDirection(currentWayPointList[currentWayPoint]);
        }
    }

    private void PlayAnimationBasedOnTargetDirection(Transform targetObject)
    {
        Vector3 direction = targetObject.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Right Direction
        if (angle > -45 && angle <= 45)
        {
            PlayAnimation(AnimatorHashToIntMoveRight);

            if (flipWhenTurnRight)
            {
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
                return;
            }

            spriteRenderer.flipY = false;
            spriteRenderer.flipX = false;
        }

        //Up Direction
        else if (angle > 45 && angle <= 135)
        {
            PlayAnimation(AnimatorHashToIntMoveUp);
            spriteRenderer.flipY = true;
            spriteRenderer.flipX = false;


        }
        //Left Direction
        else if (angle > 135 || angle <= -135)
        {
            PlayAnimation(AnimatorHashToIntMoveLeft);
            if (flipWhenTurnLeft)
            {
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
                return;
            }

            spriteRenderer.flipY = false;
            spriteRenderer.flipX = false;

        }
        //Down Direction
        else
        {
            PlayAnimation(AnimatorHashToIntMoveDown);

            spriteRenderer.flipY = false;
            spriteRenderer.flipX = false;

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
        UpdateHealth();
        PoolSystem.Singleton.SpawnFromPool(hitVFXPrefab, transform.position, Quaternion.identity);
        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                AddCoinReward();

                UIManager.Singleton.UpdateCoinUI();
                RemoveThisEnemyFromEnemyAlive();

                if (GameManager.Singleton.CheckIfNoEnemyLeft())
                    GameManager.Singleton.OnWaveCompleted_Invoke();
            }
            isDead = true;
            gameObject.SetActive(false);

        }
    }

    private void UpdateHealth()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        float healthBarSize = WORLDBAR_SCALE * healthPercentage;
        healthBar.SetSize(healthBarSize);
    }

    private void RemoveThisEnemyFromEnemyAlive()
    {
        GameManager.Singleton.enemiesAlive.Remove(this);
    }

    private void AddCoinReward()
    {
        GameManager.Singleton.AddCoin(enemyStatsSO.coinReward);
    }

    public void OnObjectSpawn()
    {
        GameManager.Singleton.enemiesAlive.Add(this);
        EnemySetup();

        // UpdateHealth();
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
