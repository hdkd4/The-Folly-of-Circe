using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform main_camera;
    private Vector3 new_position;
    // Update is called once per frame
    void Update()
    {
        new_position = main_camera.position;
        if (Input.GetKey("w"))
        {
            new_position.y += 0.01f;
            main_camera.position = new_position;
        }
        if (Input.GetKey("a"))
        {
            new_position.x -= 0.01f;
            main_camera.position = new_position;
        }
        if (Input.GetKey("s"))
        {
            new_position.y -= 0.01f;
            main_camera.position = new_position;
        }
        if (Input.GetKey("d"))
        {
            new_position.x += 0.01f;
            main_camera.position = new_position;
        }
    }
}
