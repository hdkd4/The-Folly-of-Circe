using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(DiceManager))]
//public class DiceManagerEditor : Editor
//{
//    
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//
//        DiceManager dm = (DiceManager)target;
//
//        if (GUILayout.Button("D2"))
//        {
//            dm.D2Add();
//            dm.DiceRolled();
//        }
//
//        if (GUILayout.Button("D3"))
//        {
//            dm.D3Add();
//            dm.DiceRolled();
//        }
//
//        if (GUILayout.Button("D4"))
//        {
//            dm.D4Add();
//            dm.DiceRolled();
//        }
//
//        if (GUILayout.Button("D6"))
//        {
//            dm.D6Add();
//            dm.DiceRolled();
//        }
//
//        if (GUILayout.Button("D8"))
//        {
//            dm.D8Add();
//            dm.DiceRolled();
//        }
//
//        if (GUILayout.Button("D10"))
//        {
//            dm.D10Add();
//            dm.DiceRolled();
//        }
//
//        if (GUILayout.Button("D12"))
//        {
//            dm.D12Add();
//            dm.DiceRolled();
//        }
//
//        if (GUILayout.Button("D20"))
//        {
//            dm.D20Add();
//            dm.DiceRolled();
//        }
//
//        if (GUILayout.Button("D100"))
//        {
//            dm.D100Add();
//            dm.DiceRolled();
//        }
//
//        if (GUILayout.Button("Roll"))
//        {
//            dm.amount_rolled = dm.Roll();
//        }
//    }
//
//}
namespace DiceManager
{
public class DiceManager : MonoBehaviour
{
    public string dice_rolled;
    //private int dice_sum = 0;
    public static int amount_rolled;
    private int D2_rolls = 0;
    private int D3_rolls = 0;
    private int D4_rolls = 0;
    private int D6_rolls = 0;
    private int D8_rolls = 0;
    private int D10_rolls = 0;
    private int D12_rolls = 0;
    private int D20_rolls = 0;
    private int D100_rolls = 0;
    private string D2_rolled;
    private string D3_rolled;
    private string D4_rolled;
    private string D6_rolled;
    private string D8_rolled;
    private string D10_rolled;
    private string D12_rolled;
    private string D20_rolled;
    private string D100_rolled;
    public static int D2(int times_rolled)
    {
        int sum = 0;
        for (int i = 0; i < times_rolled; i++)
        {
            sum += Random.Range(0, 2) + 1;
        }
        return sum;
    }

    public static int D3(int times_rolled = 0)
    {
        int sum = 0;
        for (int i = 0; i < times_rolled; i++)
        {
            sum += Random.Range(0, 3) + 1;
        }
        return sum;
    }

    public static int D4(int times_rolled)
    {
        int sum = 0;
        for (int i = 0; i < times_rolled; i++)
        {
            sum += Random.Range(0, 4) + 1;
        }
        return sum;
    }

    public static int D6(int times_rolled)
    {
        int sum = 0;
        for (int i = 0; i < times_rolled; i++)
        {
            sum += Random.Range(0, 6) + 1;
        }
        return sum;
    }

    public static int D8(int times_rolled)
    {
        int sum = 0;
        for (int i = 0; i < times_rolled; i++)
        {
            sum += Random.Range(0, 8) + 1;
        }
        return sum;
    }

    public static int D10(int times_rolled)
    {
        int sum = 0;
        for (int i = 0; i < times_rolled; i++)
        {
            sum += Random.Range(0, 10) + 1;
        }
        return sum;
    }

    public static int D12(int times_rolled)
    {
        int sum = 0;
        for (int i = 0; i < times_rolled; i++)
        {
            sum += Random.Range(0, 12) + 1;
        }
        return sum;
    }

    public static int D20(int times_rolled)
    {
        int sum = 0;
        for (int i = 0; i < times_rolled; i++)
        {
            sum += Random.Range(0, 20) + 1;
        }
        return sum;
    }

    public static int D100(int times_rolled)
    {
        int sum = 0;
        for (int i = 0; i < times_rolled; i++)
        {
            sum += Random.Range(0, 100) + 1;
        }
        return sum;
    }
    public void D2Add()
    {
        D2_rolls++;
        D2_rolled = D2_rolls + "d2";
    }

    public void D3Add()
    {
        D3_rolls++;
        D3_rolled = " + " + D3_rolls + "d3";
    }

    public void D4Add()
    {
        D4_rolls++;
        D4_rolled = " + " + D4_rolls + "d4";
    }

    public void D6Add()
    {
        D6_rolls++;
        D6_rolled = " + " + D6_rolls + "d6";
    }

    public void D8Add()
    {
        D8_rolls++;
        D8_rolled = " + " + D8_rolls + "d8";
    }

    public void D10Add()
    {
        D10_rolls++;
        D10_rolled = " + " + D10_rolls + "d10";
    }

    public void D12Add()
    {
        D12_rolls++;
        D12_rolled = " + " + D12_rolls + "d12";
    }

    public void D20Add()
    {
        D20_rolls++;
        D20_rolled = " + " + D20_rolls + "d20";
    }

    public void D100Add()
    {
        D100_rolls++;
        D100_rolled = " + " + D100_rolls + "d100";
    }

    //public static int Roll()
    //{
    //    dice_sum += D2(D2_rolls);
    //    dice_sum += D3(D3_rolls);
    //    dice_sum += D4(D4_rolls);
    //    dice_sum += D6(D6_rolls);
    //    dice_sum += D8(D8_rolls);
    //    dice_sum += D10(D10_rolls);
    //    dice_sum += D12(D12_rolls);
    //    dice_sum += D20(D20_rolls);
    //    dice_sum += D100(D100_rolls);
//
    //    amount_rolled = dice_sum;
    //    dice_sum = 0;
//
    //    D2_rolls = 0;
    //    D3_rolls = 0;
    //    D4_rolls = 0;
    //    D6_rolls = 0;
    //    D8_rolls = 0;
    //    D10_rolls = 0;
    //    D12_rolls = 0;
    //    D20_rolls = 0;
    //    D100_rolls = 0;
//
    //    D2_rolled = "";
    //    D3_rolled = "";
    //    D4_rolled = "";
    //    D6_rolled = "";
    //    D8_rolled = "";
    //    D10_rolled = "";
    //    D12_rolled = "";
    //    D20_rolled = "";
    //    D100_rolled = "";
    //    dice_rolled = "";
//
    //    return amount_rolled;
    //}
    public void DiceRolled()
    {
        dice_rolled = D2_rolled + D3_rolled+D4_rolled+D6_rolled+D8_rolled+D10_rolled+D12_rolled + D20_rolled + D100_rolled;
    }
}
}
