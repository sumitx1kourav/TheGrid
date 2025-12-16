using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles player input, pathfinding, and turn-based movement on the grid
public class PlayerMovement : MonoBehaviour
{
    public GridManager gridManager;     // Access to grid and tile data
    public EnemyAI enemy;               // Reference to enemy to trigger its turn
    public float moveSpeed = 3f;

    bool isMoving = false;              // Prevents new input while moving
    TileData currentTile;               // Tile the player is currently on

    // Exposes player position to other systems such as Enemy AI
    public TileData CurrentTile => currentTile;

    // Initializes player on a fixed starting tile
    void Start()
    {
        currentTile = gridManager.grid[0, 0];
        transform.position = currentTile.transform.position + Vector3.up * 0.5f;
    }

    // Handles player input and blocks interaction during movement
    void Update()
    {
        if (isMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            TryMove();
        }
    }

    // Detects clicked tile and attempts to move the player if valid
    void TryMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TileData targetTile = hit.collider.GetComponent<TileData>();

            if (targetTile != null && !targetTile.isBlocked)
            {
                List<TileData> path = FindPath(currentTile, targetTile);

                if (path != null)
                {
                    StartCoroutine(MoveAlongPath(path));
                }
            }
        }
    }

    // Uses BFS to find the shortest valid path on the grid
    List<TileData> FindPath(TileData start, TileData target)
    {
        Queue<TileData> queue = new Queue<TileData>();
        Dictionary<TileData, TileData> cameFrom = new Dictionary<TileData, TileData>();

        queue.Enqueue(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            TileData current = queue.Dequeue();
            if (current == target) break;

            foreach (TileData neighbor in GetNeighbors(current))
            {
                if (neighbor.isBlocked || cameFrom.ContainsKey(neighbor))
                    continue;

                queue.Enqueue(neighbor);
                cameFrom[neighbor] = current;
            }
        }

        if (!cameFrom.ContainsKey(target))
            return null;

        List<TileData> path = new List<TileData>();
        TileData temp = target;

        while (temp != start)
        {
            path.Add(temp);
            temp = cameFrom[temp];
        }

        path.Reverse();
        return path;
    }

    // Returns valid non-diagonal neighboring tiles for grid movement
    List<TileData> GetNeighbors(TileData tile)
    {
        List<TileData> neighbors = new List<TileData>();
        int x = tile.x;
        int y = tile.y;

        if (x > 0) neighbors.Add(gridManager.grid[x - 1, y]);
        if (x < gridManager.width - 1) neighbors.Add(gridManager.grid[x + 1, y]);
        if (y > 0) neighbors.Add(gridManager.grid[x, y - 1]);
        if (y < gridManager.height - 1) neighbors.Add(gridManager.grid[x, y + 1]);

        return neighbors;
    }

    // Moves the player smoothly along the calculated path
    // Enemy turn is triggered after player movement completes
    IEnumerator MoveAlongPath(List<TileData> path)
    {
        isMoving = true;

        foreach (TileData tile in path)
        {
            Vector3 targetPos = tile.transform.position + Vector3.up * 0.5f;

            while (Vector3.Distance(transform.position, targetPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPos,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }

            currentTile = tile;
        }

        isMoving = false;

        enemy.TakeTurn();   // Triggers enemy movement after player turn
    }
}
