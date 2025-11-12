using UnityEngine;

public class MyLegoAnimControllor : MonoBehaviour
{

    [SerializeField] private Animator legoAnimator;

    public void PlayLegoAnimationWalk()
    {
        legoAnimator.Play("Walk");
    }
    public void PlayLegoAnimatorJump()
    {
        legoAnimator.Play("Jump");
    }
}
