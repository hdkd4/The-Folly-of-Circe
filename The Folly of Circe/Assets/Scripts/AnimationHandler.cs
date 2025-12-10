using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator animator;
    Vector3 pastPos;

    void Start()
    {
        pastPos = transform.position;
    }
    void Update()
    {
        if(pastPos == transform.position && !animator.GetBool("AttackLeft") && !animator.GetBool("AttackRight"))
        {
            animator.SetBool("MovUp", false);
            animator.SetBool("MovDown", false);
            animator.SetBool("MovLeft", false);
            animator.SetBool("MovRight", false);
            animator.SetBool("Idle", true);
        }

        if(transform.position.y > pastPos.y)
        {
            animator.SetBool("MovUp", true);
            animator.SetBool("MovDown", false);
            animator.SetBool("MovLeft", false);
            animator.SetBool("MovRight", false);
            animator.SetBool("Idle", false);
        }
        else if(transform.position.y < pastPos.y)
        {
            animator.SetBool("MovUp", false);
            animator.SetBool("MovDown", true);
            animator.SetBool("MovLeft", false);
            animator.SetBool("MovRight", false);
            animator.SetBool("Idle", false);
        }
        else if(transform.position.x < pastPos.x)
        {
            animator.SetBool("MovUp", false);
            animator.SetBool("MovDown", false);
            animator.SetBool("MovLeft", true);
            animator.SetBool("MovRight", false);
            animator.SetBool("Idle", false);
        }
        else if(transform.position.x > pastPos.x)
        {
            animator.SetBool("MovUp", false);
            animator.SetBool("MovDown", false);
            animator.SetBool("MovLeft", false);
            animator.SetBool("MovRight", true);
            animator.SetBool("Idle", false);
        }
        pastPos = transform.position;
    }
}
