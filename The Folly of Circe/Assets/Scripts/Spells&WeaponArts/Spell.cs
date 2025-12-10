using System.Collections;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public virtual IEnumerator Cast() { yield break; }
}
