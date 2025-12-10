using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    public GameObject menu;
    public void Toggle()
    {
        if (menu.activeSelf == true)
            menu.SetActive(false);
        else
            menu.SetActive(true);
    }
    
}
