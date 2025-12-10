using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform shakenObject;
    public int shakeTime;

    public IEnumerator ShakeScreen()
    {
        Vector3 pos = shakenObject.position;
        Quaternion rot = shakenObject.rotation;
        for(int i = 0; i < shakeTime; i++)
        {
            float xVal = Random.Range(-150, 150) / 100;
            float yVal = Random.Range(-150, 150) / 100;
            shakenObject.position = new Vector3(pos.x + xVal/4, pos.y + yVal/4, pos.z);
            shakenObject.rotation = new Quaternion(rot.x + xVal/80, rot.y, rot.z + yVal/80, rot.w + yVal/80);
            yield return new WaitForEndOfFrame();
            shakenObject.rotation = rot;
            shakenObject.position = pos;
        }
    }
    
    public void StartShake()
    {
        StartCoroutine(ShakeScreen());
    }
}
