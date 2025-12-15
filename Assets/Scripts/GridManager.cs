using UnityEngine;

// Responsible for creating the grid
public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float spacing = 1.05f;
    public GameObject tilePrefab;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // This loop creates the grid of tiles based on width and height
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

                tile.name = $"Tile_{x}_{y}";
            }
        }
    }
}