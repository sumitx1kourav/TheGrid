using UnityEngine;

// Spawns obstacles and marks tiles as blocked
public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GridManager gridManager;
    public GameObject obstaclePrefab;
    public float spacing = 1.05f;

    void Start()
    {
        SpawnObstacles();
    }

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

                    Instantiate(obstaclePrefab, pos, Quaternion.identity);

                    // ?? Mark the tile as blocked
                    TileData tile = gridManager.grid[x, y];
                    tile.isBlocked = true;
                }
            }
        }
    }
}
