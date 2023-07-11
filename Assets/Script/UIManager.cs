using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Singleton;
    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] TMP_Text levelInfo_TMP;
    [SerializeField] TMP_Text waveLevel_TMP;

    [SerializeField] TowerItemUI[] towerItemUIs;
    private TowerItemUI selectedTowerItem;

    private void Update()
    {
        if (selectedTowerItem)
        {
            Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
            spawnPosition = TowerDefenseGrid.ValidateWorldGridPosition(spawnPosition);
            spawnPosition += new Vector3(1, 1, 0) * TowerDefenseGrid.grid.GetCellSize() * .5f;

            selectedTowerItem.SetTowerPreviewPosition(spawnPosition);

            if (TowerDefenseGrid.grid.GetGridObject(spawnPosition) == null
            || TowerDefenseGrid.grid.GetGridObject(spawnPosition).GetOccupied() == true)
            {
                selectedTowerItem.HideTowerPreview();
                TowerUpgrade.Singleton.HideSensorOverlay();
            }
            else
            {
                selectedTowerItem.ShowTowerPreview();
                if (selectedTowerItem.towerPreviewPrefab != null)
                    TowerUpgrade.Singleton.ShowSensorOverlay(selectedTowerItem);
            }
        }
    }

    public void LevelSetup()
    {
        var gm = GameManager.Singleton;
        levelInfo_TMP.text = "Level: " + gm.currentLevelInfo.level.ToString();
        waveLevel_TMP.text = "Wave: " + gm.currentWave.ToString();
    }

    public void SetSelectedTowerItem(TowerItemUI newTowerItemUI)
    {
        ClearSelected();
        selectedTowerItem = newTowerItemUI;
    }

    public void ClearSelected()
    {
        if (selectedTowerItem)
            selectedTowerItem.Deselected();

        TowerUpgrade.Singleton.HideSensorOverlay();
        selectedTowerItem = null;
    }

}
