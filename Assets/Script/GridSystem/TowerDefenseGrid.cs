using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class TowerDefenseGrid : MonoBehaviour
{
    private GridSystem<TDGridNode> grid;
    [SerializeField] List<GameObject> towerPrefab = new List<GameObject>();

    [SerializeField] int width = 5;
    [SerializeField] int height = 5;
    [SerializeField] float cellSize = 5f;
    [SerializeField] TDGridNode selectedGrid;
    private void Start()
    {
        grid = new GridSystem<TDGridNode>(width, height, cellSize, transform.position, (GridSystem<TDGridNode> g, int x, int y) => new TDGridNode(g, x, y));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition();
            TDGridNode gridobj = grid.GetGridObject(pos);
            // selectedGrid = gridobj;
            if (gridobj.GetOccupied())
            {
                print("Canot Place tower here");
                return;
            }
            SpawnTower();
            gridobj.SetOccupied(true);
            gridobj.TriggerGridObjectChanged();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (selectedGrid != null)
                selectedGrid.SetWalkable(true);
        }
    }

    public void SetOccupiedWithCollider(BoxCollider2D boxCollider2D)
    {
        Vector2[] cornerPosition = new Vector2[5];
        Bounds bounds = boxCollider2D.bounds;

        // Calculate the corner positions
        Vector2 topLeftCorner = new Vector2(bounds.min.x, bounds.max.y);
        Vector2 topRightCorner = bounds.max;
        Vector2 topMiddleCorner = new Vector2(bounds.min.x / 2, bounds.max.y);
        Vector2 bottomLeftCorner = bounds.min;
        Vector2 bottomRightCorner = new Vector2(bounds.max.x, bounds.min.y);

        // Print the corner positions

        cornerPosition[0] = topLeftCorner;
        cornerPosition[1] = topRightCorner;
        cornerPosition[2] = bottomLeftCorner;
        cornerPosition[3] = bottomRightCorner;
        cornerPosition[4] = topMiddleCorner;

        for (int i = 0; i < cornerPosition.Length; i++)
        {
            grid.GetGridObject(cornerPosition[i]).SetOccupied(true);
            grid.GetGridObject(cornerPosition[i]).TriggerGridObjectChanged();
        }

        for (int i = 0; i < cornerPosition.Length; i++)
        {
            print(grid.GetGridObject(cornerPosition[i]) + " " + grid.GetGridObject(cornerPosition[i]).GetOccupied());
        }

        Debug.Log("Top Left Corner: " + topLeftCorner);
        Debug.Log("Top Right Corner: " + topRightCorner);
        Debug.Log("Bottom Left Corner: " + bottomLeftCorner);
        Debug.Log("Bottom Right Corner: " + bottomRightCorner);
    }

    private void SpawnTower()
    {
        Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
        spawnPosition = ValidateWorldGridPosition(spawnPosition);
        spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;

        var tower = Instantiate(towerPrefab[0], spawnPosition, Quaternion.identity);
        SetOccupiedWithCollider(tower.GetComponent<BoxCollider2D>());
    }


    private Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return grid.GetWorldPosition(x, y);
    }

}

[System.Serializable]
public class TDGridNode
{
    GridSystem<TDGridNode> grid;
    private int x;
    private int y;
    private bool walkable;
    private bool occupied;
    private int nodeIndex;

    public TDGridNode(GridSystem<TDGridNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void TriggerGridObjectChanged()
    {
        grid.TriggerGridObjectChanged(x, y);

    }

    public override string ToString()
    {
        return nodeIndex.ToString() + " \n" + occupied;
    }

    public void SetNodeIndex(int index)
    {
        nodeIndex = index;
        TriggerGridObjectChanged();
    }

    public int GetNodeIndex()
    {
        return nodeIndex;
    }

    public bool GetWalkable()
    {
        return walkable;
    }

    public void SetWalkable(bool value)
    {
        walkable = value;
        TriggerGridObjectChanged();
    }

    public bool GetOccupied()
    {
        return occupied;
    }

    public void SetOccupied(bool value)
    {
        occupied = value;
    }






}