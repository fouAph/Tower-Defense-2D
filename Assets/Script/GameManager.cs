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

    [SerializeField] GameObject selectedTower;
    [SerializeField] GameObject[] towerForPreview;
    [SerializeField] public GameObject selectedPreviewTower;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition();
            TDGridNode gridobj = TowerDefenseGrid.grid.GetGridObject(pos);
            // selectedGrid = gridobj;
            if (gridobj == null) return;
            if (gridobj.GetOccupied())
            {
                print("Canot Place tower here");
                return;
            }
            TowerDefenseGrid.SpawnTower();
            gridobj.SetOccupied(true);
            gridobj.TriggerGridObjectChanged();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UIManager.Singleton.ClearSelected();
            selectedTower = null;
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            Vector3 pos = CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition();
            TDGridNode gridobj = TowerDefenseGrid.grid.GetGridObject(pos);
            gridobj.LogDebug();
        }
    }


    public GameObject GetSelectedTower()
    {
        return selectedTower;
    }

    public void SetSelectedTower(GameObject towerPrefab)
    {
        selectedTower = towerPrefab;
    }
}