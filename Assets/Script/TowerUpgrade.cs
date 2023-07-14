using UnityEngine;
using CodeMonkey.Utils;
public class TowerUpgrade : MonoBehaviour
{
    public static TowerUpgrade Singleton;
    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] GameObject sensorOverlay;
    [SerializeField] private Tower tower;
    [SerializeField] Button_Sprite upgradeSpriteButton;
    [SerializeField] Button_Sprite deleteSpriteButton;

    private void Start()
    {
        upgradeSpriteButton.ClickFunc = UpgradeTower;
    }


    public void UpdateSensorOverlay(TowerStatsSO towerStatsSO)
    {
        sensorOverlay.transform.localScale = Vector2.one * towerStatsSO.sensorRadius * 2;
    }

    public void ShowSensorOverlay(Tower _tower)
    {
        tower = _tower;
        UpdateSensorOverlay(_tower.towerStatsSO);
        transform.localPosition = _tower.transform.position + tower.GetSensorOffset();
        sensorOverlay.gameObject.SetActive(true);
        ShowUpgradeButton();
    }

    public void ShowSensorOverlay(TowerItemUI towerItemUI)
    {
        TowerStatsSO towerStatsSO = towerItemUI.towerStatsSO;
        UpdateSensorOverlay(towerStatsSO);
        transform.localPosition = towerItemUI.towerPreviewPrefab.transform.position + towerItemUI.towerPrefab.GetSensorOffset();
        sensorOverlay.gameObject.SetActive(true);
        HideUpgradeButton();
    }

    public void ShowUpgradeButton()
    {
        deleteSpriteButton.gameObject.SetActive(true);
        if (!tower.CheckIfMaxUpgrade())
        {
            upgradeSpriteButton.gameObject.SetActive(true);
        }
    }

    public void HideUpgradeButton()
    {
        deleteSpriteButton.gameObject.SetActive(false);
        upgradeSpriteButton.gameObject.SetActive(false);
    }

    public void HideSensorOverlay()
    {
        sensorOverlay.gameObject.SetActive(false);
    }

    private void UpgradeTower()
    {
        tower.UpgradeTower();
        if (tower.CheckIfMaxUpgrade())
        {
            upgradeSpriteButton.gameObject.SetActive(false);
        }
        UpdateSensorOverlay(tower.towerStatsSO);
        print("tower " + tower.name);
    }

    //FIX Sensor OverLay not updating when Upgrade
}

