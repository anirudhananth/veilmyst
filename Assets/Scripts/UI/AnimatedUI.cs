using UnityEngine;

public class AnimatedUI : Showable
{
    public bool Show = false;
    public bool HideByDefault = true;

    public Animator Animator;

    public override void SetShow(bool show)
    {
        if (!Animator.gameObject.activeSelf) Animator.gameObject.SetActive(show);
        Animator.SetBool("show", show);
    }

    public override void Toggle()
    {
        Show = !Show;
        SetShow(Show);
    }

    private void Start()
    {
        if (Animator == null) Animator = GetComponent<Animator>();
        else if (!Show && HideByDefault) Animator.gameObject.SetActive(false);
        SetShow(Show);
    }
}