using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SpawnDummy : MonoBehaviour
{
    public GameObject dummy;
    public Vector2 spawnLocation;
    public GameObject newDummy;
    public TurnManager turnManager;

    public void Spawn()
    {
        Destroy(newDummy);
        newDummy = Instantiate(dummy, spawnLocation, Quaternion.identity);
        turnManager.AddToList(newDummy.GetComponent<StatsManager>());
    }
}
