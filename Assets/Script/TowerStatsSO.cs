using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "Tower Defense/TowerStatsSO", order = 0)]
public class TowerStatsSO : ScriptableObject
{
    public int towerPrice = 100;
    public float fireRate = 3f;
    public float sensorRadius = 20f;
    public int weaponDamage = 1;

    public Sprite[] towerLevelSprite;
    public TowerStatsUpgrade[] towerStatsUpgrades;

    public bool CanBuyTower(int coin)
    {
        return coin >= towerPrice;
    }

    public void SetTowerSprite(SpriteRenderer towerSprite, int level)
    {
        towerSprite.sprite = towerLevelSprite[level - 1];
    }

    [System.Serializable]
    public class TowerStatsUpgrade
    {
        public float fireRateUpgrade = 4f;
        public float sensorRadiusUpgrade = 25f;
        public int weaponDamageUpgrade = 3;

    }
}

