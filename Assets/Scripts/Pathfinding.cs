using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Vector2Int start = new Vector2Int(0, 1);
    public Vector2Int goal = new Vector2Int(4, 4);
    public int width = 4;
    public int height = 4;
    public float obstacleProbability = .25f;
    public Vector2Int newObstacle = new Vector2Int(0, 0);

    private List<Vector2Int> path = new List<Vector2Int>();
    private Vector2Int next;
    private Vector2Int current;

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    private int[,] grid;

    private void Start()
    {
        RegenerateGrid();
    }

    public void RegenerateGrid()
    {
        int attempts = 0;
        int maxAttempts = 100;
        bool validPath = false;

        while (!validPath && attempts < maxAttempts)
        {
            CreateRandomGrid(width, height, obstacleProbability);
            validPath = FindPath(start, goal);
            attempts++;
        }

        if (!validPath)
        {
            Debug.Log("Couldn't make valid path");
        }
    }

    public void AddObstacle()
    {
        if (grid != null && newObstacle != start && newObstacle != goal)
        {
            grid[newObstacle.y, newObstacle.x] = 1;
            FindPath(start, goal);
        }
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            float cellSize = 1f;

            // Draw grid cells
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 cellPosition = new Vector3(x * cellSize, 0, y * cellSize);
                    Gizmos.color = grid[y, x] == 1 ? Color.black : Color.white;
                    Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
                }
            }

            // Draw path
            foreach (var step in path)
            {
                Vector3 cellPosition = new Vector3(step.x * cellSize, 0, step.y * cellSize);
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
            }

            // Draw start and goal
            Gizmos.color = Color.green;
            Gizmos.DrawCube(new Vector3(start.x * cellSize, 0, start.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));

            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(goal.x * cellSize, 0, goal.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));
        }
    }

    private bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.x < grid.GetLength(1) && point.y >= 0 && point.y < grid.GetLength(0);
    }

    private bool FindPath(Vector2Int start, Vector2Int goal)
    {
        path.Clear();

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(start);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach (Vector2Int direction in directions)
            {
                next = current + direction;

                if (IsInBounds(next) && grid[next.y, next.x] == 0 && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(goal))
        {
            Debug.Log("Path not found.");
            return false;
        }

        // Trace path from goal to start
        Vector2Int step = goal;
        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        path.Add(start);
        path.Reverse();

        return true;
    }

    void CreateRandomGrid(int width, int height, float obstacleProbability)
    {
        grid = new int[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                // Don't make an obstacle on start or end
                if (pos == start || pos == goal)
                {
                    continue;
                }

                grid[y,x] = Random.value < obstacleProbability ? 1 : 0;
            }
        }
    }
}
