using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy AI that moves towards the player
public class EnemyAI : MonoBehaviour, IAI
{
    public GridManager gridManager;
    public PlayerMovement player;
    public float moveSpeed = 3f;

    TileData currentTile;
    bool isMoving = false;

    void Start()
    {
        currentTile = gridManager.grid[
            gridManager.width - 1,
            gridManager.height - 1
        ];

        transform.position = currentTile.transform.position + Vector3.up * 0.5f;
    }

    public void TakeTurn()
    {
        if (isMoving) return;

        TileData targetTile = GetClosestAdjacentTile();
        if (targetTile == null) return;

        List<TileData> path = FindPath(currentTile, targetTile);
        if (path != null)
            StartCoroutine(MoveAlongPath(path));
    }

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
