using UnityEngine;

public class EndProgram : MonoBehaviour
{
    public GameObject pauseMenu;
    public void Kill()
    {
        Application.Quit();
    }

    public void GoAway()
    {
        pauseMenu.SetActive(false);
    }
}
