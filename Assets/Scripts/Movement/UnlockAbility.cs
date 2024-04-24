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
    public string inputName;
    public GameObject Player;
    PlayerInput playerInput;

    // UI vars
    [Header("UI vars")]
    public TMP_Text uiText;
    public string Instruction;
    public float helpTime = 0;
    private bool helped = false;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = Player.GetComponent<PlayerInput>();
        playerInput.actions.FindAction(inputName).Disable(); 
        uiText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!helped)
        {
            helpTime += Time.time;
            helped = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Time.time >= helpTime)
        {
            playerInput.actions.FindAction(inputName).Enable();
            uiText.text = Instruction;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            uiText.text = "";
        }
    }
}
