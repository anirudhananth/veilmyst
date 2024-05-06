using UnityEngine;

public class AnimatedUI : Showable
{
    public bool Show = false;
    public bool HideByDefault = true;

    public Animator Animator;
    public CanvasGroup group;

    public override void SetShow(bool show)
    {
        if (group && show && HideByDefault) group.alpha = 1;
        if (!Animator.gameObject.activeSelf) Animator.gameObject.SetActive(show);
        Animator.SetBool("show", show);
        Show = show;
    }

    public override void Toggle()
    {
        SetShow(!Show);
    }

    private void Start()
    {
        if (Animator == null) Animator = GetComponent<Animator>();
        else if (!Show && HideByDefault)
        {
            if (TryGetComponent(out group))
            {
                group.alpha = 0;
            }
            else
            {
                Animator.gameObject.SetActive(false);
            }
        }
        SetShow(Show);
    }
}