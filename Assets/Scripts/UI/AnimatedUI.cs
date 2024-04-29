using UnityEngine;

public class AnimatedUI : Showable
{
    public bool show = false;

    public Animator Animator;

    public override void SetShow(bool show)
    {
        if (!Animator.gameObject.activeSelf) Animator.gameObject.SetActive(show);
        Animator.SetBool("show", show);
    }

    public override void Toggle()
    {
        show = !show;
        SetShow(show);
    }

    private void Start()
    {
        if (Animator == null) Animator = GetComponent<Animator>();
        else if (!show) Animator.gameObject.SetActive(false);
        SetShow(show);
    }
}