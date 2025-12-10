using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(PlayerMove))]
//public class PlayerMoveEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        PlayerMove pm = (PlayerMove)target;
//
//        if (GUILayout.Button("Reset"))
//            pm.Reset();
//    }
//}
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float moveDistanceMax = 9.0f;
    public float moveDistanceTracker;
    public Transform moveTracker;
    public Transform selfTransform;

    // Update is called once per frame
    void Update()
    {
        if (moveTracker.position == selfTransform.position) { }
        else
        {
            selfTransform.position = Vector3.MoveTowards(selfTransform.position, moveTracker.position, moveSpeed * Time.deltaTime);
            if (moveDistanceTracker > 0)
            {
                moveDistanceTracker -= moveSpeed * Time.deltaTime;
            }
        }
    }

    public float GetMoveTracker()
    {
        return moveDistanceTracker;
    }

    public void Reset()
    {
        moveDistanceTracker = moveDistanceMax;
    }
}
