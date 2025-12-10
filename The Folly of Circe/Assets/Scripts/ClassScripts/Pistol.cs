using UnityEngine;

public class Pistol : Weapon
{
    public override void Use()
    {
        Debug.Log("Bang! 9mm round fired!");
    }

    public void Equiped()
    {
        Debug.Log("Equiped: " + gameObject.name);
    }
}
