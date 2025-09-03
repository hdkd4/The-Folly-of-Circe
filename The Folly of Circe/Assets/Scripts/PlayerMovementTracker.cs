using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovementTracker : MonoBehaviour
{
    public Transform move_to_circle;
    private Vector2 mouse_screen_position;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            mouse_screen_position = Input.mousePosition;
            Vector2 mouse_world_position = Camera.main.ScreenToWorldPoint(mouse_screen_position);
            move_to_circle.transform.position = mouse_world_position;
        }
    }
}
