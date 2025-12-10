using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MakeDamageNumbers : MonoBehaviour
{
    public GameObject damageNumberPrefab;
    public CameraFollowTarget cameraFollowTarget;

    public IEnumerator MakeDamageNumber(string damage, Vector2 position, bool important)
    {
        GameObject dmgNumber = Instantiate(damageNumberPrefab, position, Quaternion.identity);
        TextMeshProUGUI text = dmgNumber.GetComponentInChildren<TextMeshProUGUI>();
        text.text = damage;
        dmgNumber.transform.position = position;
        Vector2 newPos = new Vector2(position.x, position.y+1);
        dmgNumber.transform.position = position;
        dmgNumber.transform.DOMove(newPos, 0.0f, true);
        if (important)
        {
            cameraFollowTarget.overrideTarget = dmgNumber.transform;
            cameraFollowTarget.follow = true;
        }
        yield return new WaitForSeconds(1);
        if(important)
            cameraFollowTarget.overrideTarget = null;
        Destroy(dmgNumber);
    }
}
