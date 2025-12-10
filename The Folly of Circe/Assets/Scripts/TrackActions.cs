using TMPro;
using UnityEngine;

public class TrackActions : MonoBehaviour
{
    public StatsManager playerStats;
    public TextMeshProUGUI actionPointsText;
    private int actionPoints;
    void Update()
    {
        actionPoints = playerStats.actionPoints;
        actionPointsText.text = actionPoints.ToString() + "/" + playerStats.actionPointsMax;
    }
}
