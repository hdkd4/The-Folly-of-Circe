using TMPro;
using UnityEngine;

public class TrackMovement : MonoBehaviour
{
    public TurnManager turnManager;
    public TextMeshProUGUI moveDistanceText;
    private float moveDistance;
    void Update()
    {
        moveDistance = turnManager.moveDistanceTracker;
        moveDistanceText.text = moveDistance.ToString("F1");
    }
}
