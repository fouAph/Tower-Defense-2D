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
    [SerializeField] Button_Sprite upgradeSpriteButton;
    [SerializeField] Button_Sprite deleteSpriteButton;
    [SerializeField] TextMesh upgradePrice_TextMesh;

    private Tower tower;

    private void Start()
    {
        upgradeSpriteButton.ClickFunc = UpgradeTower;
        deleteSpriteButton.ClickFunc = DeleteTower;
        HideSensorOverlay();
    }

    public void UpdateSensorOverlayForPreview(TowerStatsSO towerStatsSO)
    {
        sensorOverlay.transform.localScale = Vector2.one * towerStatsSO.baseSensorRadius * 2;
    }

    public void UpdateSensorOverlay(Tower _tower)
    {
        sensorOverlay.transform.localScale = Vector2.one * _tower.GetSensorRadius() * 2;
    }

    public void ShowSensorOverlay(Tower _tower)
    {
        tower = _tower;
        UpdateSensorOverlay(_tower);
        transform.localPosition = _tower.transform.position + tower.GetSensorOffset();
        sensorOverlay.gameObject.SetActive(true);
        ShowUpgradeButton();
    }

    public void ShowSensorOverlayForPreview(TowerItemUI towerItemUI)
    {
        TowerStatsSO towerStatsSO = towerItemUI.towerStatsSO;
        UpdateSensorOverlayForPreview(towerStatsSO);
        transform.localPosition = towerItemUI.towerPreviewPrefab.transform.position + towerItemUI.towerPrefab.GetSensorOffset();
        sensorOverlay.gameObject.SetActive(true);
        HideUpgradeButton();
    }

    public void ShowUpgradeButton()
    {
        deleteSpriteButton.gameObject.SetActive(true);
        UpdateUpgradePrice();
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
        if (!tower.CanUpgradeTower())
        {
            CodeMonkey.CMDebug.TextPopupMouse("Not Enough Coin To Upgrade ");
            return;
        }
        GameManager.Singleton.SubstractCoin(tower.towerStatsSO.towerStatsUpgrades[tower.GetCurrentTowerLevel()].upgradePrice);
        UIManager.Singleton.UpdateCoinUI();
        tower.UpgradeTower();
        if (tower.CheckIfMaxUpgrade())
        {
            upgradeSpriteButton.gameObject.SetActive(false);
        }
        UpdateSensorOverlay(tower);
        UpdateUpgradePrice();
    }

    private void UpdateUpgradePrice()
    {
        if (tower.GetCurrentTowerLevel() != tower.towerStatsSO.towerStatsUpgrades.Length)
            upgradePrice_TextMesh.text = "$" + tower.towerStatsSO.towerStatsUpgrades[tower.GetCurrentTowerLevel()].upgradePrice.ToString();
    }

    private void DeleteTower()
    {
        TowerDefenseGrid.ResetOccupiedWithCollider(tower.GetComponent<BoxCollider2D>());
        UIManager.Singleton.ClearSelected();
        GameManager.Singleton.SetSelectedTower(null);
        TDGridNode gridobj = TowerDefenseGrid.grid.GetGridObject(tower.transform.position);
        gridobj.SetOccupied(false);
        tower.gameObject.SetActive(false);
        tower = null;
        HideSensorOverlay();
    }
}

