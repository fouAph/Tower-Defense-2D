using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "Tower Defense/TowerStatsSO", order = 0)]
public class TowerStatsSO : ScriptableObject
{
    public int towerPrice = 100;
    public float fireRate = 3f;
    public float sensorRadius = 20f;
    public int weaponDamage = 1;

    public TowerStatsUpgrade[] towerStatsUpgrades;

    public bool CanBuyTower(int coin)
    {
        return coin >= towerPrice;
    }

    public bool CanUpgradeTower(int coin, int towerLevel)
    {
        return coin >= towerStatsUpgrades[towerLevel - 1].upgradePrice;
    }

    public void SetTowerSprite(SpriteRenderer towerSprite, int towerLevel)
    {
        towerSprite.sprite = towerStatsUpgrades[towerLevel - 1].towerLevelSprite;
    }


    [System.Serializable]
    public class TowerStatsUpgrade
    {
        public int upgradePrice = 300;
        public float fireRateUpgrade = 4f;
        public float sensorRadiusUpgrade = 25f;
        public int weaponDamageUpgrade = 3;

        public Sprite towerLevelSprite;
    }
}

