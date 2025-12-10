using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform mainCamera;
    private Vector3 newPosition;
    // Update is called once per frame
    void Update()
    {
        newPosition = mainCamera.position;
        if (Input.GetKey("w"))
        {
            newPosition.y += 0.02f;
            mainCamera.position = newPosition;
        }
        if (Input.GetKey("a"))
        {
            newPosition.x -= 0.02f;
            mainCamera.position = newPosition;
        }
        if (Input.GetKey("s"))
        {
            newPosition.y -= 0.02f;
            mainCamera.position = newPosition;
        }
        if (Input.GetKey("d"))
        {
            newPosition.x += 0.02f;
            mainCamera.position = newPosition;
        }
    }
}
