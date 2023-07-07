using UnityEngine;

public class Grid : MonoBehaviour
{
    public int rows;
    public int columns;
    public float cellSize;
    public GameObject cellPrefab;

    private void Start()
    {
        // Calculate the size of the grid based on the screen size
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 gridDimensions = CalculateGridDimensions(screenSize);

        // Create the grid
        CreateGrid((int)gridDimensions.x, (int)gridDimensions.y);
    }

    private Vector2 CalculateGridDimensions(Vector2 screenSize)
    {
        float screenWidth = screenSize.x;
        float screenHeight = screenSize.y;

        // Calculate the number of cells that can fit horizontally and vertically
        int horizontalCells = Mathf.FloorToInt(screenWidth / cellSize);
        int verticalCells = Mathf.FloorToInt(screenHeight / cellSize);

        // Calculate the actual dimensions of the grid
        float gridWidth = horizontalCells * cellSize;
        float gridHeight = verticalCells * cellSize;

        // Return the grid dimensions
        return new Vector2(gridWidth, gridHeight);
    }
    
    private void CreateGrid(int width, int height)
    {
        if (cellSize <= 0f)
        {
            Debug.LogError("Invalid cell size!");
            return;
        }

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                // Create a new cell GameObject
                GameObject cell = new GameObject("Cell");
                cell.transform.parent = transform;

                // Position the cell
                Vector3 position = new Vector3(column * cellSize, row * cellSize, 0f);
                cell.transform.localPosition = position;

                // Add a SpriteRenderer to visualize the cell
                SpriteRenderer spriteRenderer = cell.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("CellSprite"); // Replace "CellSprite" with your desired sprite

                // Add a TextMesh component to display the cell index
                TextMesh textMesh = cell.AddComponent<TextMesh>();
                textMesh.text = string.Format("({0},{1})", row, column);
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignment.Center;
            }
        }
    }
}
