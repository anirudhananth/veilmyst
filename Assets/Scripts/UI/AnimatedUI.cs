using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatedUI : Showable
{
    public bool show = false;

    private Animator animator;

    public override void SetShow(bool show)
    {
        animator.SetBool("show", show);
    }

    public override void Toggle()
    {
        show = !show;
        SetShow(show);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetShow(show);
    }
}