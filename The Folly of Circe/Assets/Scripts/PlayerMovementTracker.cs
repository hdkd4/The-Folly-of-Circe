using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovementTracker : MonoBehaviour
{
    public Transform moveToCircle;
    public Transform moveToPreview;
    public PlayerMove player;
    private Vector2 mouseScreenPosition;
    public bool moving = true;

    void Update()
    {
        if (Object.FindFirstObjectByType<TurnManager>().state != BattleState.PlayerTurn)
            return;
        if (moving == true)
        {
            moveToPreview.position = player.transform.position;
            mouseScreenPosition = Input.mousePosition;
            Vector2 mouse_world_position = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            moveToPreview.position = Vector3.MoveTowards(player.transform.position, mouse_world_position, player.GetMoveTracker());
            if (Input.GetMouseButtonDown(0))
            {
                moveToCircle.position = player.transform.position;
                mouseScreenPosition = Input.mousePosition;
                mouse_world_position = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                if(player.GetMoveTracker()>0)
                    moveToCircle.position = Vector3.MoveTowards(moveToCircle.position, mouse_world_position, player.GetMoveTracker());
            }
        }
    }
}
