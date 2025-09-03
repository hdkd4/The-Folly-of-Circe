using UnityEngine;

public class RightClickMenu : MonoBehaviour
{
    public GameObject context_menu_panel;
    private Vector2 mouse_screen_position;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            mouse_screen_position = Input.mousePosition;
            Vector2 mouse_world_position = Camera.main.ScreenToWorldPoint(mouse_screen_position);
            context_menu_panel.SetActive(true);
            context_menu_panel.transform.position = mouse_world_position;
        }
        if (Input.GetMouseButtonDown(0) && context_menu_panel.activeSelf)
        {
            context_menu_panel.SetActive(false);
        }
    }
    public void Option1Clicked()
    {
        context_menu_panel.SetActive(false);
    }
}
