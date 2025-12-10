using UnityEngine;

public class RightClickMenu : MonoBehaviour
{
    public GameObject contextMenuPanel;
    private Vector2 mouseScreenPosition;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            mouseScreenPosition = Input.mousePosition;
            Vector2 mouse_world_position = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            ShowContextMenu(mouse_world_position);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(contextMenuPanel.activeSelf)
                contextMenuPanel.SetActive(false);
        }
    }
    void ShowContextMenu(Vector2 _position)
    {
        contextMenuPanel.SetActive(true);
        contextMenuPanel.transform.position = _position;
    }
    public void Option1Clicked()
    {
        contextMenuPanel.SetActive(false);
    }
}
