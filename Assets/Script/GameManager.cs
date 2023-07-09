using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    private int coin = 500;
    public int Coin
    {
        get { return coin; }
        private set { coin = value; }
    }
    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] GameObject selectedTowerGO;

    private void Update()
    {
        if (selectedTowerGO)
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 pos = CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition();
                TDGridNode gridobj = TowerDefenseGrid.grid.GetGridObject(pos);
                // selectedGrid = gridobj;
                if (gridobj == null) return;

                /*if (gridobj.GetOccupied())
                {
                   
                    return;
                }*/

                SpawnTower();
                gridobj.SetOccupied(true);
                gridobj.TriggerGridObjectChanged();
            }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UIManager.Singleton.ClearSelected();
            selectedTowerGO = null;
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            Vector3 pos = CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition();
            TDGridNode gridobj = TowerDefenseGrid.grid.GetGridObject(pos);
            gridobj.LogDebug();
        }
    }

    public void SpawnTower()
    {
        TowerDefenseGrid.SpawnTower();
    }

    public GameObject GetSelectedTowerGO()
    {
        return selectedTowerGO;
    }

    public void SetSelectedTower(GameObject towerPrefab)
    {
        selectedTowerGO = towerPrefab;
    }
}