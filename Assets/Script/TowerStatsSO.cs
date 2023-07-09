using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "Tower Defense/TowerUpgrade", order = 0)]
public class TowerStatsSO : ScriptableObject
{
    public float fireRate = 3f;
    public float sensorRadius = 20f;
    public int weaponDamage = 1;
}

