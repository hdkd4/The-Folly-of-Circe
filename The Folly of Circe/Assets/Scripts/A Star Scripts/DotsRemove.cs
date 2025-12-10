using UnityEngine;

public class DotsRemove : MonoBehaviour
{
    public GameObject dot;
    public CircleCollider2D dotCollider;
    public LayerMask killLayers;

    void Update()
    {
        if (dotCollider.IsTouchingLayers(killLayers))
        {
            Destroy(dot);
        }
    }
}
