using UnityEngine;

public class CreateMoveDots : MonoBehaviour
{
    public Pathfinding pathfinding;
    public GameObject dotPrefab;
    public GameObject moveDots;
    public float dotSpacing = 0.2f;

    [Header("Dot Colors")]
    public Color reachableColor = Color.white;
    public Color blockedColor = Color.red;

    public Vector2 finalReachablePosition { get; private set; }

    public void MakeDots(Vector2 start, Vector2 target, float maxMoveDistance)
    {
        foreach (Transform child in moveDots.transform)
            Destroy(child.gameObject);

        var path = pathfinding.FindPath(start, target);

        if (path != null)
        {
            Vector2 lastPos = start;
            float usedDistance = 0f;
            finalReachablePosition = start;

            foreach (Node node in path)
            {
                Vector2 nextPos = node.worldPosition;
                float segmentDistance = Vector2.Distance(lastPos, nextPos);

                for (float d = 0; d < segmentDistance; d += dotSpacing)
                {
                    Vector2 dotPos = Vector2.Lerp(lastPos, nextPos, d / segmentDistance);
                    usedDistance += dotSpacing;

                    GameObject dot = Instantiate(dotPrefab, moveDots.transform);
                    dot.transform.position = dotPos;

                    var renderer = dot.GetComponent<SpriteRenderer>();
                    if (renderer != null)
                    {
                        if (usedDistance <= maxMoveDistance)
                        {
                            renderer.color = reachableColor;
                            finalReachablePosition = dotPos;
                        }
                        else
                        {
                            renderer.color = blockedColor;
                        }
                    }
                }
                lastPos = nextPos;
            }
        }
    }

    public void DestroyDots()
    {
        foreach (Transform child in moveDots.transform)
            Destroy(child.gameObject);
    }
}
