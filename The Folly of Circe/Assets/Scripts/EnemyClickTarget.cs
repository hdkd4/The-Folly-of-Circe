using UnityEngine;

public class EnemyClickTarget : MonoBehaviour
{
    private StatsManager stats;
    private TurnManager turnManager;

    void Start()
    {
        stats = GetComponent<StatsManager>();
        turnManager = Object.FindFirstObjectByType<TurnManager>();
    }

    void OnMouseDown()
    {
        if (stats != null && turnManager != null)
        {
            turnManager.SelectEnemyTarget(stats);
        }
    }
}