using Unity.VisualScripting;
using UnityEngine;

public class AddToEnemies : MonoBehaviour
{
    public Transform enemiesList;
    public TurnManager turnManager;

    public void MoveToList()
    {
        foreach(Transform childTransform in enemiesList)
        {
            GameObject enemy = childTransform.gameObject;
            turnManager.AddToList(enemy.GetComponent<StatsManager>());
        }
    }
}
