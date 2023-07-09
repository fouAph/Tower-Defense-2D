using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "Tower Defense/TowerStatsSO", order = 0)]
public class TowerStatsSO : ScriptableObject
{
    public float fireRate = 3f;
    public float sensorRadius = 20f;
    public int weaponDamage = 1;
}

