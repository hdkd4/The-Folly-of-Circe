using UnityEngine;

public class GridManager : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;

    float nodeDiameter;
    int gridSizex, gridSizey;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizex = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizey = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizex, gridSizey];
        Vector2 worldBottomLeft = (Vector2)transform.position -
                                  Vector2.right * gridWorldSize.x / 2 -
                                  Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizex; x++)
        {
            for (int y = 0; y < gridSizey; y++)
            {
                Vector2 worldPoint = worldBottomLeft +
                                     Vector2.right * (x * nodeDiameter + nodeRadius) +
                                     Vector2.up * (y * nodeDiameter + nodeRadius);

                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentx = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percenty = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentx = Mathf.Clamp01(percentx);
        percenty = Mathf.Clamp01(percenty);

        int x = Mathf.RoundToInt((gridSizex - 1) * percentx);
        int y = Mathf.RoundToInt((gridSizey - 1) * percenty);
        return grid[x, y];
    }

    public System.Collections.Generic.List<Node> GetNeighbors(Node node)
    {
        var neighbors = new System.Collections.Generic.List<Node>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int checkx = node.gridx + dx;
                int checky = node.gridy + dy;

                if (checkx >= 0 && checkx < gridSizex && checky >= 0 && checky < gridSizey)
                    neighbors.Add(grid[checkx, checky]);
            }
        }
        return neighbors;
    }

    public void UpdateDynamicObstacles(Transform player, StatsManager[] enemies)
    {
        // Rebuild static obstacle map
        foreach (Node n in grid)
        {
            n.walkable = !Physics2D.OverlapCircle(n.worldPosition, nodeRadius, unwalkableMask);
        }

        // Mark area under the player as blocked
        BlockAreaUnderTransform(player);

        // Mark areas under each enemy
        foreach (var e in enemies)
        {
            if (e == null || e.health <= 0) continue;
            BlockAreaUnderTransform(e.transform);
        }
    }
    
    private void BlockAreaUnderTransform(Transform t)
    {
        float radius = 0.5f; // tweak based on your character collider size
        Collider2D[] hits = Physics2D.OverlapCircleAll(t.position, radius);
    
        foreach (Collider2D hit in hits)
        {
            Node node = NodeFromWorldPoint(hit.transform.position);
            if (node != null) node.walkable = false;
        }
    }
}
