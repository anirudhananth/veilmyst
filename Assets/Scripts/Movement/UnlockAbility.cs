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

    // UI vars
    [Header("UI vars")]
    public TMP_Text uiText;
    public string Instruction;

    private bool helped = false;
    private Animator animator;
    private string activeInstruction;

    // Start is called before the first frame update
    void Start()
    {
        activeInstruction = Instruction;
        uiText.GetComponent<TextMeshProUGUI>().text = "";
        animator = uiText.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enable the action if not before
        if (!helped)
        {
            helped = true;
            if (InputActionRef != null) InputActionRef.action.Enable();
        }

        // Replace the placeholder text with the key binding display string
        if (InputActionRef != null)
        {
            string inputName = InputActionRef.action.GetBindingDisplayString(0);
            activeInstruction = Instruction.Replace("KEY", inputName);
        }
        uiText.text = activeInstruction;
        animator.SetBool("show", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (uiText.text == activeInstruction)
            {
                // This check accounts for the case when the player goes to the
                // next scene.
                if (animator != null) {
                    animator.SetBool("show", false);
                }
            }
        }
    }
}
