using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls enemy behaviour using grid-based pathfinding.
// The enemy takes a turn only after the player finishes moving.
public class EnemyAI : MonoBehaviour, IAI
{
    public GridManager gridManager;        // Reference to the grid for tile access
    public PlayerMovement player;          // Reference to player to track position
    public float moveSpeed = 3f;

    TileData currentTile;                  // Tile the enemy is currently standing on
    bool isMoving = false;                 // Used to prevent multiple moves at once

    // Initializes enemy position on a fixed starting tile (top-right corner)
    void Start()
    {
        currentTile = gridManager.grid[
            gridManager.width - 1,
            gridManager.height - 1
        ];

        transform.position = currentTile.transform.position + Vector3.up * 0.5f;
    }

    // Called externally by the player after finishing movement.
    // This ensures turn-based behaviour.
    public void TakeTurn()
    {
        if (isMoving) return;

        TileData targetTile = GetClosestAdjacentTile();
        if (targetTile == null) return;

        List<TileData> path = FindPath(currentTile, targetTile);
        if (path != null)
            StartCoroutine(MoveAlongPath(path));
    }

    // Selects the closest valid tile adjacent to the player.
    // This ensures the enemy does not move onto the player tile directly.
    TileData GetClosestAdjacentTile()
    {
        TileData playerTile = player.CurrentTile;
        TileData bestTile = null;
        float bestDistance = float.MaxValue;

        foreach (TileData tile in GetNeighbors(playerTile))
        {
            if (tile.isBlocked) continue;

            float dist = Vector3.Distance(
                tile.transform.position,
                transform.position
            );

            if (dist < bestDistance)
            {
                bestDistance = dist;
                bestTile = tile;
            }
        }

        return bestTile;
    }

    // Performs grid-based BFS pathfinding to calculate a valid path.
    // BFS is used because all tiles have equal movement cost.
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

    // Returns all valid non-diagonal neighboring tiles for grid movement.
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

    // Moves the enemy smoothly along the calculated path.
    // Movement is handled using a coroutine for visible tile-by-tile motion.
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
    }
}
