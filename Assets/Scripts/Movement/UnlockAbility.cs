using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UnlockAbility : MonoBehaviour
{
    // Player Input vars
    [Header("Player Input Vars")]
    public InputActionReference InputActionRef;
    public GameObject Player;
    public bool DisableActionUpfront;

    // UI vars
    [Header("UI vars")]
    public TMP_Text TextBox;
    public string Instruction;

    private Showable animator;
    private string activeInstruction;

    // Start is called before the first frame update
    void Start()
    {
        activeInstruction = Instruction;
        TextBox.text = "";
        animator = TextBox.GetComponent<Showable>();
        if (DisableActionUpfront) InputActionRef.action.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        // Enable the action if not before
        if (DisableActionUpfront && InputActionRef != null && !InputActionRef.action.enabled)
        {
            InputActionRef.action.Enable();
        }

        // Replace the placeholder text with the key binding display string
        if (InputActionRef != null)
        {
            string inputName = InputActionRef.action.GetBindingDisplayString(0);
            activeInstruction = Instruction.Replace("KEY", inputName);
        }
        TextBox.text = activeInstruction;
        animator.SetShow(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        if (TextBox.text != activeInstruction) return;

        // This check accounts for the case when the player goes to the
        // next scene.
        if (animator != null)
        {
            animator.SetShow(false);
        }
    }
}
