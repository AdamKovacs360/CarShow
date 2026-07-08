using UnityEngine;

public class anim : MonoBehaviour
{
    public Animator MyAnimator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(input.GetKeyDown("z"))
        {
            MyAnimator.SetTrigger("button_clicked");
        }
    }
}
