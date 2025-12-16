using UnityEngine;

// Responsible for reading obstacle data and applying it to the grid at runtime
public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;      // ScriptableObject containing blocked tile data
    public GridManager gridManager;        // Reference to grid to mark tiles as blocked
    public GameObject obstaclePrefab;      // Visual representation of obstacles
    public float spacing = 1.05f;

    // Initializes obstacle placement when the scene starts
    void Start()
    {
        SpawnObstacles();
    }

    // Spawns obstacle objects and updates tile data based on stored obstacle configuration
    void SpawnObstacles()
    {
        for (int x = 0; x < obstacleData.width; x++)
        {
            for (int y = 0; y < obstacleData.height; y++)
            {
                if (obstacleData.IsBlocked(x, y))
                {
                    Vector3 pos = new Vector3(
                        x * spacing,
                        0.25f,
                        y * spacing
                    );

                    // Creates a visible obstacle at the blocked tile position
                    Instantiate(obstaclePrefab, pos, Quaternion.identity);

                    // Marks the corresponding tile as blocked for pathfinding and movement
                    TileData tile = gridManager.grid[x, y];
                    tile.isBlocked = true;
                }
            }
        }
    }
}
