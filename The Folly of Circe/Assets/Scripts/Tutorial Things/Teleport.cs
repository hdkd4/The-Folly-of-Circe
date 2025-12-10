using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Vector3 goPos;
    public void Go(Transform transform)
    {
        transform.position = goPos;
    }
}
