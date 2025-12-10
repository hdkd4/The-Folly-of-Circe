using UnityEngine;

public class Shotgun : Weapon
{
    public override void Use()
    {
        Debug.Log("Boom! Shotgun round fired!");
    }

    public void Equiped()
    {
        Debug.Log("Equiped: " + gameObject.name);
    }
}
