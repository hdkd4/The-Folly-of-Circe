using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GridManager grid;

    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        var openSet = new List<Node>();
        var closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                 openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                int newCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    int GetDistance(Node a, Node b)
    {
        int dstx = Mathf.Abs(a.gridx - b.gridx);
        int dsty = Mathf.Abs(a.gridy - b.gridy);

        if (dstx > dsty)
            return 14 * dsty + 10 * (dstx - dsty);
        return 14 * dstx + 10 * (dsty - dstx);
    }

    public List<Node> FindPathToTarget(Vector2 start, Vector2 target)
    {
        Node targetNode = grid.NodeFromWorldPoint(target);
        bool originalState = targetNode.walkable;

        targetNode.walkable = true;

        var path = FindPath(start, target);

        targetNode.walkable = originalState;

        if (path != null && path.Count > 1)
        {
            path.RemoveAt(path.Count - 2); // remove final node (the player tile)
        }

        return path;
    }

    public List<Node> ReachableNodes(Vector2 startPos, float movementRange)
{
    Node startNode = grid.NodeFromWorldPoint(startPos);
    var reachableNodes = new List<Node>();
    var queue = new Queue<Node>();
    var visited = new HashSet<Node>();

    // Reset all node costs before running
    foreach (Node n in grid.grid)
    {
        n.gCost = int.MaxValue;
        n.parent = null;
    }

    // Early exit if starting tile is blocked
    if (!startNode.walkable)
    {
        startNode.walkable = true;
    }

    startNode.gCost = 0;
    queue.Enqueue(startNode);
    visited.Add(startNode);

    while (queue.Count > 0)
    {
        Node currentNode = queue.Dequeue();
        reachableNodes.Add(currentNode);

        foreach (Node neighbor in grid.GetNeighbors(currentNode))
        {
            if (!neighbor.walkable || visited.Contains(neighbor))
                continue;

            // Distance between adjacent nodes — assume ~10 for cardinal, 14 for diagonal
            int stepCost = GetDistance(currentNode, neighbor);
            int newCost = currentNode.gCost + stepCost;

            // Check if within movement range
            if (newCost <= movementRange * 50f) // *10 to match A* scale
            {
                neighbor.gCost = newCost;
                neighbor.parent = currentNode;
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
            }
        }
    }

    return reachableNodes;
}

}
