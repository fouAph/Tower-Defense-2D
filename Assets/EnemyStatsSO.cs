using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Tower Defense/EnemyStats", order = 0)]
public class EnemyStatsSO : ScriptableObject
{
    public string enemyName;
    public int maxHealth = 100;
    public float moveSpeed = 5f;
    public int attackDamage = 2;

    public int coinReward = 100;
}