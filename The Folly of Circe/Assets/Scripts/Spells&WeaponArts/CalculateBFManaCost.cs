using UnityEngine;

public class CalculateBFManaCost : MonoBehaviour
{
    public int totalCost;
    public int CalculateCost(int power, float radius, bool isRanged)
    {
        totalCost = power/2 + Mathf.RoundToInt(radius);
        if (isRanged)
        {
            totalCost += 5; // Additional cost for ranged spells/arts
        }
        return totalCost;
    }
}
