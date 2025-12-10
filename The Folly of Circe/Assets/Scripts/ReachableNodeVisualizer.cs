using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways] // lets it run in Editor Scene view too
public class ReachableNodeVisualizer : MonoBehaviour
{
    [Header("References")]
    public Pathfinding pathfinding;
    public Transform selfTransform;

    [Header("Settings")]
    public float movementRange = 5f;
    public Color reachableColor = new Color(0f, 1f, 0f, 0.3f);
    public Color blockedColor = new Color(1f, 0f, 0f, 0.3f);
    public float nodeRadius = 0.2f;

    private List<Node> reachableNodes = new List<Node>();

    void OnDrawGizmos()
    {
        if (pathfinding == null || pathfinding.grid == null) return;

        // Get reachable nodes using your function
        reachableNodes = pathfinding.ReachableNodes(selfTransform.position, movementRange);

        foreach (Node node in pathfinding.grid.grid)
        {
            Vector3 worldPos = node.worldPosition;
            if (reachableNodes.Contains(node))
                Gizmos.color = reachableColor;
            else if (!node.walkable)
                Gizmos.color = blockedColor;
            else
                Gizmos.color = new Color(1f, 1f, 1f, 0.1f); // faint for normal nodes

            Gizmos.DrawSphere(worldPos, nodeRadius);
        }

        // Draw the unit itself
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(selfTransform.position, 0.25f);
        Debug.Log($"[Visualizer] ReachableNodes Count: {reachableNodes.Count}");
    }
}
