using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public Transform target;
    public Transform overrideTarget;
    public bool follow = false;
    private Vector3 targetPos;
    void Update()
    {
        if (overrideTarget != null)
        {
            targetPos = new Vector3(overrideTarget.position.x, overrideTarget.position.y, transform.position.z);
        }
        else if (target != null)
        {
            targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
        if (follow)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 50f * Time.deltaTime);
        if (!follow || target == null)
            return;
    }
}
