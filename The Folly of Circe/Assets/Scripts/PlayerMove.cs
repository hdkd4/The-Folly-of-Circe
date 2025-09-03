using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

public class PlayerMove : MonoBehaviour
{
    public float move_speed = .01f;
    public float move_distance_max = 9.0f;
    private float move_distance_tracker;
    public Transform move_tracker;
    public Transform self_transform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move_tracker.position == self_transform.position)
        {
            move_distance_tracker = move_distance_max;
        }
        else
        {
            if (move_distance_tracker > 0)
            {
                self_transform.position = Vector3.MoveTowards(self_transform.position, move_tracker.position, move_speed);
                move_distance_tracker -= move_speed;
            }
            else
            {
                move_tracker.position = self_transform.position;
                move_distance_tracker = move_distance_max;
            }
        }
    }
}
