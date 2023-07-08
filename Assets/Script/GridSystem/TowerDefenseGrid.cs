using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Tilemaps;

public class TowerDefenseGrid : MonoBehaviour
{
    public static TowerDefenseGrid Singleton;

    [SerializeField] Tilemap NotPlaceableTilemap; 
    private void Awake()
    {
        Singleton = this;
        grid = new GridSystem<TDGridNode>(width, height, cellSize, transform.position, (GridSystem<TDGridNode> g, int x, int y) => new TDGridNode(g, x, y));
    }
    public static GridSystem<TDGridNode> grid;
    [SerializeField] List<GameObject> towerPrefab = new List<GameObject>();

    [SerializeField] int width = 5;
    [SerializeField] int height = 5;
    [SerializeField] float cellSize = 5f;
    [SerializeField] TDGridNode selectedGrid;

    private void Start()
    {
        SetOccupiedInTilemap();
    }

    public void SetOccupiedInTilemap()
    {
        BoundsInt bounds = NotPlaceableTilemap.cellBounds;
        Vector3 cellSize = NotPlaceableTilemap.cellSize;
        Vector3 origin = NotPlaceableTilemap.transform.position;

        // Initialize a list to store the world positions
        List<Vector3> occupiedCoordinates = new List<Vector3>();

        // Iterate over the tiles within the bounds
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                // Get the position of the current tile
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // Check if the tile exists at the current position
                if (NotPlaceableTilemap.HasTile(cellPosition))
                {
                    // Convert the cell position to world position
                    Vector3 worldPosition = NotPlaceableTilemap.CellToWorld(cellPosition);

                    // Adjust the world position based on cell size and origin
                    worldPosition += cellSize * 0.5f + origin;

                    // Add the world position to the list
                    occupiedCoordinates.Add(worldPosition);
                }
            }
        }

        // Print the list of world positions
        foreach (Vector3 coordinate in occupiedCoordinates)
        {
            if (grid.GetGridObject(coordinate) != null)
            {
                grid.GetGridObject(coordinate).SetOccupied(true);
            }

        }
    }

    public static void SetOccupiedWithCollider(BoxCollider2D boxCollider2D)
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
            if (grid.GetGridObject(cornerPosition[i]) != null)
                grid.GetGridObject(cornerPosition[i]).SetOccupied(true);
        }

        // for (int i = 0; i < cornerPosition.Length; i++)
        // {
        //     if (grid.GetGridObject(cornerPosition[i]) != null)
        //     print(grid.GetGridObject(cornerPosition[i]) + " " + grid.GetGridObject(cornerPosition[i]).GetOccupied());
        // }

        // Debug.Log("Top Left Corner: " + topLeftCorner);
        // Debug.Log("Top Right Corner: " + topRightCorner);
        // Debug.Log("Bottom Left Corner: " + bottomLeftCorner);
        // Debug.Log("Bottom Right Corner: " + bottomRightCorner);
    }

    public static void SpawnTower()
    {
        Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
        spawnPosition = ValidateWorldGridPosition(spawnPosition);
        spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;

        var tower = Instantiate(GameManager.Singleton.GetSelectedTower(), spawnPosition, Quaternion.identity);
        SetOccupiedWithCollider(tower.GetComponent<BoxCollider2D>());
    }

    public static Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return grid.GetWorldPosition(x, y);
    }

    public TDGridNode GetSelectedGrid()
    {
        return selectedGrid;
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
        return occupied.ToString();
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
        TriggerGridObjectChanged();
    }

    public void LogDebug()
    {
        Debug.Log("Location: " + new Vector2(x, y) + "Walkable: " + walkable + "Occupied: " + occupied);
    }






}