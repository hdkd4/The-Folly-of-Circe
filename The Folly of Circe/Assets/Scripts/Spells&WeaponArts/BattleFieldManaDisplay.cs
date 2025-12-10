using UnityEngine;

public class BattleFieldManaDisplay : MonoBehaviour
{
    public int battleFieldMana = 15;
    public TMPro.TMP_Text manaText;
    void Update()
    {
        manaText.text = battleFieldMana.ToString();
    }

    public void ReduceMana(int amount)
    {
        battleFieldMana -= amount;
    }

    public void IncreaseMana(int amount)
    {
        battleFieldMana += amount;
    }

    public int GetCurrentMana()
    {
        return battleFieldMana;
    }

    public void ResetMana()
    {
        battleFieldMana = 15;
    }
}
