using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Enemy : MonoBehaviour, IDamageable, IPooledObject
{
    private const int WORLDBAR_SCALE = 14;
    [SerializeField] EnemyStatsSO enemyStatsSO;
    [SerializeField] List<Transform> currentWayPointList = new List<Transform>();
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        healthBar = new World_Bar(transform, new Vector3(-0.219999999f, 0.300000012f, 0), Vector3.one, Color.white, Color.red, WORLDBAR_SCALE, 15);

        AnimatorHashToIntMoveUp = Animator.StringToHash("MoveUp");
        AnimatorHashToIntMoveDown = Animator.StringToHash("MoveDown");
        AnimatorHashToIntMoveLeft = Animator.StringToHash("MoveLeft");
        AnimatorHashToIntMoveRight = Animator.StringToHash("MoveRight");

        EnemySetup();

        PlayAnimationBasedOnTargetDirection(currentWayPointList[currentWayPoint]);
    }

    private void EnemySetup()
    {
        maxHealth = enemyStatsSO.health;
        moveSpeed = enemyStatsSO.moveSpeed;
        attackDamage = enemyStatsSO.attackDamage;

        currentHealth = maxHealth;
    }



    private void PlayAnimation(int stringHash)
    {
        animator.Play(stringHash);
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
            PlayAnimationBasedOnTargetDirection(currentWayPointList[currentWayPoint]);

            if (currentWayPoint >= currentWayPointList.Count)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void PlayAnimationBasedOnTargetDirection(Transform targetObject)
    {
        Vector3 direction = targetObject.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Right Direction
        if (angle > -45 && angle <= 45)
            PlayAnimation(AnimatorHashToIntMoveRight);

        //Up Direction
        else if (angle > 45 && angle <= 135)
            PlayAnimation(AnimatorHashToIntMoveUp);

        //Left Direction
        else if (angle > 135 || angle <= -135)
            PlayAnimation(AnimatorHashToIntMoveLeft);

        //Right Direction
        else
            PlayAnimation(AnimatorHashToIntMoveDown);

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
