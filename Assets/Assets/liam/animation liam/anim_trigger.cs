using UnityEngine;

public class KeyAnimationTrigger : MonoBehaviour
{
    public Animator animator;
    public KeyCode triggerKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            animator.SetTrigger("carInteract");
        }
    }
}