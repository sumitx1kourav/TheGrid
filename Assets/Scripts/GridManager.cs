using UnityEngine;

// Responsible for creating the grid
public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float spacing = 1.05f;
    public GameObject tilePrefab;
    public TileData[,] grid;

    void Start()
    {
        GenerateGrid();
    }
    // Instantiates tiles and assigns grid coordinates and references
    // This grid acts as the foundation for pathfinding and AI logic
    void GenerateGrid()
    {
        grid = new TileData[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = Instantiate(
                    tilePrefab,
                    new Vector3(x * spacing, 0, y * spacing),
                    Quaternion.identity
                );

                TileData data = tile.GetComponent<TileData>();
                data.x = x;
                data.y = y;
                data.isBlocked = false;

                // Stores tile reference for fast access during gameplay systems
                grid[x, y] = data;

                tile.name = $"Tile_{x}_{y}";
            }
        }
    }
}
