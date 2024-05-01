using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public MenuItem[] MenuItems;
    public PlayerInput PlayerInput;
    public TextMeshProUGUI HintText;
    public AudioClip SelectAudioClip;
    public AudioClip ConfirmAudioClip;
    public bool HasFocus = false;

    private InputAction upAction;
    private InputAction downAction;
    private InputAction enterAction;
    private bool initialized = false;

    private int selectedItemIndex = -1;
    private const float inputDelay = .03f;
    private float inputDelayTimer = 0;

    private AudioSource audioSource;

    private void Init()
    {
        initialized = true;
        MenuItems = GetComponentsInChildren<MenuItem>();
        upAction = PlayerInput.actions["Up"];
        downAction = PlayerInput.actions["Down"];
        enterAction = PlayerInput.actions["Enter"];
        string upStr = upAction.GetBindingDisplayString(0);
        string downStr = downAction.GetBindingDisplayString(0);
        string clickStr = enterAction.GetBindingDisplayString(0);
        HintText.text = $"[{upStr}] Select Up | [{downStr}] Select Down | [{clickStr}] Select";

        Debug.Assert(MenuItems.Length > 0);
        selectedItemIndex = -1;
        Select(0);
        // Set the audio source later to not trigger sound effect on the first
        // select
        audioSource = GetComponent<AudioSource>();
    }

    private void Select(int index)
    {
        if (selectedItemIndex != -1) MenuItems[selectedItemIndex].Deselect();
        if (audioSource) {
            audioSource.clip = SelectAudioClip;
            audioSource.Play();
        }
        MenuItems[index].Select();
        selectedItemIndex = index;
    }

    private void Enter()
    {
        if (audioSource) {
            audioSource.clip = ConfirmAudioClip;
            audioSource.Play();
        }
        if (selectedItemIndex != -1) MenuItems[selectedItemIndex].Enter(this);
    }

    private void Update()
    {
        if (!initialized)
        {
            Init();
        }

        if (!HasFocus) return;

        if (selectedItemIndex == -1 || MenuItems.Length == 0) return;

        if (inputDelayTimer > 0)
        {
            inputDelayTimer -= Time.deltaTime;
            return;
        }

        int index = selectedItemIndex;
        int count = MenuItems.Length;

        if (upAction.triggered)
        {
            Select((index + count - 1) % count);
            inputDelayTimer = inputDelay;
        }
        else if (downAction.triggered)
        {
            Select((index + count + 1) % count);
            inputDelayTimer = inputDelay;
        }
        else if (enterAction.triggered)
        {
            Enter();
            inputDelayTimer = inputDelay;
        }
    }
}