using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuActionBase))]
public class MenuItem : Showable
{
    public TextMeshProUGUI textBox;
    public Animator Animator;
    public bool show = true;
    public Menu ParentMenu;
    public bool Selected { get; private set; }
    [Tooltip("Use this to replace the menu-wide confirmation sound effect")]
    public AudioClip ConfirmAudioClip;

    private MenuActionBase menuAction;
    private string text;

    public void Start()
    {
        menuAction = GetComponent<MenuActionBase>();
        text = textBox.text;
        Deselect();
        SetShow(show);
    }

    public void Select()
    {
        Selected = true;
        Animator.SetBool("selected", true);
        textBox.text = $"[{text}]";
        menuAction.Select(ParentMenu);
    }

    public void Deselect()
    {
        Selected = false;
        Animator.SetBool("selected", false);
        textBox.text = $" {text} ";
        menuAction.Deselect(ParentMenu);
    }

    public void Enter(Menu source)
    {
        Animator.SetTrigger("triggered");
        StartCoroutine(Trigger(source, 0.5f));
    }

    public override void SetShow(bool show)
    {
        base.SetShow(show);
        Animator.SetBool("show", show);
    }

    public override void Toggle()
    {
        SetShow(!show);
    }

    private IEnumerator Trigger(Menu source, float delay)
    {
        yield return new WaitForSeconds(delay);
        menuAction.Trigger(source);
    }

}