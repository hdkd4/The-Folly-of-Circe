using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplay : MonoBehaviour
{
    private StatsManager stats;
    private TextMeshProUGUI display;
    public int UnconciousLevel = 1;
    void Update()
    {
        stats = GetComponent<StatsManager>();
        display = GetComponentInChildren<TextMeshProUGUI>();

        if(stats.health == UnconciousLevel)
        {
            display.text = "Unconscious";
        }
        else if(stats.health <=0)
        {
            display.text = "Dead";
        }
        else
            display.text = stats.health + " / " + stats.maxHealth;
    }
}
