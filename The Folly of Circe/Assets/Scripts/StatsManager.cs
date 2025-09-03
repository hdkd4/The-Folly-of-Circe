using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatsManager))]
public class StatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StatsManager sm = (StatsManager)target;

        if (GUILayout.Button("Set Stat"))
        {
            sm.SetBaseStats();
            sm.SetStats();
        }
    }
}
public class StatsManager : MonoBehaviour
{
    public int stat_choice;
    public int new_score;
    private int base_strength = 10;
    private int base_dexterity = 10;
    private int base_constitution = 10;
    private int base_wisdom = 10;
    private int base_intelligence = 10;
    private int base_charisma = 10;
    private int initiative = 10;

    public int strength;
    public int dexterity;
    public int constitution;
    public int wisdom;
    public int intelligence;
    public int charisma;

    public void SetStats()
    {
        strength = base_strength;
        dexterity = base_dexterity;
        constitution = base_constitution;
        wisdom = base_wisdom;
        intelligence = base_intelligence;
        charisma = base_charisma;
    }
    public void SetBaseStats()
    {
        if (stat_choice == 0)
            base_strength = new_score;
        if (stat_choice == 1)
            base_dexterity = new_score;
        if (stat_choice == 2)
            base_constitution = new_score;
        if (stat_choice == 3)
            base_wisdom = new_score;
        if (stat_choice == 4)
            base_intelligence = new_score;
        if (stat_choice == 5)
            base_charisma = new_score;
    }
    public void StatImprove(int stat_choice)
    {
        if (stat_choice == 0)
            base_strength += 1;
        if (stat_choice == 1)
            base_dexterity += 1;
        if (stat_choice == 2)
            base_constitution += 1;
        if (stat_choice == 3)
            base_wisdom += 1;
        if (stat_choice == 4)
            base_intelligence += 1;
        if (stat_choice == 5)
            base_charisma += 1;
    }

    public void StatOverride(int stat_choice, int new_score)
    {
        if (stat_choice == 0)
            strength = new_score;
        if (stat_choice == 1)
            dexterity = new_score;
        if (stat_choice == 2)
            constitution = new_score;
        if (stat_choice == 3)
            wisdom = new_score;
        if (stat_choice == 4)
            intelligence = new_score;
        if (stat_choice == 5)
            charisma = new_score;
    }
}
