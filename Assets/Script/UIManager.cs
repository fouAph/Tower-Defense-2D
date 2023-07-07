using System.Collections;
using System.Collections.Generic;
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
