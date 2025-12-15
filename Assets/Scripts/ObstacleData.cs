using UnityEngine;

// Stores which tiles are blocked
[CreateAssetMenu(menuName = "Grid/Obstacle Data")]
public class ObstacleData : ScriptableObject
{
    public int width = 10;
    public int height = 10;

    // 1D array to store grid data (10x10 = 100)
    public bool[] blocked;

    public void Initialize()
    {
        blocked = new bool[width * height];
    }

    public bool IsBlocked(int x, int y)
    {
        return blocked[y * width + x];
    }

    public void SetBlocked(int x, int y, bool value)
    {
        blocked[y * width + x] = value;
    }
}
