using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(StatsManager))]
//public class StatsEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//
//        StatsManager sm = (StatsManager)target;
//
//        if (GUILayout.Button("Set Stat"))
//        {
//            sm.SetBaseStats();
//            sm.SetStats();
//        }
//    }
//}
public class StatsManager : MonoBehaviour
{
    public int armorClass = 10;
    public int maxHealth = 10;
    public int health = 10;
    public int statChoice;
    public int newScore;
    public int baseStrength = 10;
    public int baseDexterity = 10;
    public int baseConstitution = 10;
    public int baseWisdom = 10;
    public int baseIntelligence = 10;
    public int baseCharisma = 10;
    public int initiative = 10;

    public int strength;
    public int dexterity;
    public int constitution;
    public int wisdom;
    public int intelligence;
    public int charisma;
    public int actionPointsMax = 1;
    public int actionPoints = 1;
    public int storedMana = 25;

    public void SetStats()
    {
        strength = baseStrength;
        dexterity = baseDexterity;
        constitution = baseConstitution;
        wisdom = baseWisdom;
        intelligence = baseIntelligence;
        charisma = baseCharisma;
    }
    //public void SetBaseStats()
    //{
    //    if (statChoice == 0)
    //        baseStrength = newScore;
    //    if (statChoice == 1)
    //        baseDexterity = newScore;
    //    if (statChoice == 2)
    //        baseConstitution = newScore;
    //    if (statChoice == 3)
    //        baseWisdom = newScore;
    //    if (statChoice == 4)
    //        baseIntelligence = newScore;
    //    if (statChoice == 5)
    //        baseCharisma = newScore;
    //}
    public void StatImprove(int statChoice)
    {
        if (statChoice == 0)
            baseStrength += 1;
        if (statChoice == 1)
            baseDexterity += 1;
        if (statChoice == 2)
            baseConstitution += 1;
        if (statChoice == 3)
            baseWisdom += 1;
        if (statChoice == 4)
            baseIntelligence += 1;
        if (statChoice == 5)
            baseCharisma += 1;
    }

    public void StatOverride(int statChoice, int newScore)
    {
        if (statChoice == 0)
            strength = newScore;
        if (statChoice == 1)
            dexterity = newScore;
        if (statChoice == 2)
            constitution = newScore;
        if (statChoice == 3)
            wisdom = newScore;
        if (statChoice == 4)
            intelligence = newScore;
        if (statChoice == 5)
            charisma = newScore;
        SetStats();
    }

    public void HealthChange(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }
}
