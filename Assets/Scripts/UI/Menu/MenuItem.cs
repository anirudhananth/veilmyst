using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuActionBase))]
public class MenuItem : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public Animator Animator;
    private MenuActionBase menuAction;
    private string text;

    public void Start()
    {
        menuAction = GetComponent<MenuActionBase>();
        text = textBox.text;
        Deselect();
    }

    public void Select()
    {
        Animator.SetBool("selected", true);
        textBox.text = $"[{text}]";
    }

    public void Deselect()
    {
        Animator.SetBool("selected", false);
        textBox.text = $" {text} ";
    }

    public void Enter()
    {
        Animator.SetTrigger("triggered");
        StartCoroutine(Trigger(0.5f));
    }

    private IEnumerator Trigger(float delay)
    {
        yield return new WaitForSeconds(delay);
        menuAction.Trigger();
    }

}