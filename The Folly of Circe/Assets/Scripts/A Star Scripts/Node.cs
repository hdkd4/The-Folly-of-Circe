using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridx, gridy;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool walkable, Vector2 worldPos, int x, int y)
    {
        this.walkable = walkable;
        this.worldPosition = worldPos;
        gridx = x;
        gridy = y;
    }

    public int fCost => gCost + hCost;
}
