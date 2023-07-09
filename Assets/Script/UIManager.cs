using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton;
    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] TowerItemUI[] towerItemUIs;
    [SerializeField] TowerItemUI selectedTowerItem;

    private void Update()
    {
        if (selectedTowerItem)
        {
            Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
            spawnPosition = TowerDefenseGrid.ValidateWorldGridPosition(spawnPosition);
            spawnPosition += new Vector3(1, 1, 0) * TowerDefenseGrid.grid.GetCellSize() * .5f;

            selectedTowerItem.SetTowerPreviewPosition(spawnPosition);

            if (TowerDefenseGrid.grid.GetGridObject(spawnPosition) == null || TowerDefenseGrid.grid.GetGridObject(spawnPosition).GetOccupied() == true)
            {
                selectedTowerItem.HideTowerPreview();
            }
            else{
                selectedTowerItem.ShowTowerPreview();
            }
        }
    }
    public void SetSelectedTowerItem(TowerItemUI newTowerItemUI)
    {
        if (selectedTowerItem)
            selectedTowerItem.Deselected();
        selectedTowerItem = newTowerItemUI;
    }


    public void ClearSelected()
    {
        if (selectedTowerItem)
            selectedTowerItem.Deselected();
    }

}
