using System;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Priority
{
    public const int Default = 10;
    public const int Menu = 12;
    public const int MenuOverride = 14;
}

public enum HintVariant
{
    Level, Default, Volume, RebindNormal, RebindReset
}

public class Menu : Showable
{
    public MenuItem[] MenuItems;
    public PlayerInput PlayerInput;
    public TextMeshProUGUI HintText;
    public AudioClip SelectAudioClip;
    public AudioClip ConfirmAudioClip;
    public bool HasFocus = false;
    public Menu ParentMenu;
    public AnimatedUI Animator;
    public CinemachineVirtualCamera MenuCamera;

    public InputAction UpAction, DownAction, LeftAction, RightAction, EnterAction;
    private bool initialized = false;

    private int selectedItemIndex = -1;
    private const float inputDelay = .03f;
    private float inputDelayTimer = 0;

    private AudioSource audioSource;

    public override void SetShow(bool show)
    {
        HasFocus = show;
        if (MenuCamera != null) MenuCamera.Priority = show ? Priority.Menu : Priority.Default;
        foreach (MenuItem item in MenuItems)
        {
            item.SetShow(show);
        }
        if (Animator) Animator.SetShow(show);
    }

    public override void Toggle()
    {
        SetShow(!HasFocus);
    }

    public void OverrideHint(string text)
    {
        HintText.text = text;
    }

    public void SetHintText(HintVariant variant)
    {
        string upStr = UpAction.GetBindingDisplayString(0);
        string downStr = DownAction.GetBindingDisplayString(0);
        string clickStr = EnterAction.GetBindingDisplayString(0);
        string leftStr = LeftAction.GetBindingDisplayString(0);
        string rightStr = RightAction.GetBindingDisplayString(0);

        switch (variant)
        {
            case HintVariant.Default:
                HintText.text = $"[{upStr}] Select Up | [{downStr}] Select Down | [{clickStr}] Select";
                break;
            case HintVariant.Level:
                HintText.text = $"[{leftStr}] Prev level | [{rightStr}] Next level | [{clickStr}] Play | [{upStr}/{downStr}] ...";
                break;
            case HintVariant.Volume:
                HintText.text = $"[{leftStr}] Lower Volume | [{rightStr}] Higher Volume | [{clickStr}] Test Audio | [{upStr}/{downStr}] ...";
                break;
            case HintVariant.RebindNormal:
                HintText.text = $"[{rightStr}] Switch to Reset | [{clickStr}] Start Rebind | [{upStr}/{downStr}] ...";
                break;
            case HintVariant.RebindReset:
                HintText.text = $"[{leftStr}] Rebind | [{clickStr}] Reset to Default | [{upStr}/{downStr}] ...";
                break;
        }
    }

    private void Init()
    {
        initialized = true;
        MenuItems = GetComponentsInChildren<MenuItem>();
        foreach (MenuItem item in MenuItems)
        {
            item.ParentMenu = this;
        }
        UpAction = PlayerInput.actions["Up"];
        DownAction = PlayerInput.actions["Down"];
        LeftAction = PlayerInput.actions["Left"];
        RightAction = PlayerInput.actions["Right"];
        EnterAction = PlayerInput.actions["Enter"];
        SetHintText(HintVariant.Default);
        SetShow(HasFocus);

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
        if (audioSource)
        {
            audioSource.clip = SelectAudioClip;
            audioSource.Play();
        }
        MenuItems[index].Select();
        selectedItemIndex = index;
    }

    private void Enter()
    {
        if (audioSource)
        {
            audioSource.clip = ConfirmAudioClip;
            if (MenuItems[selectedItemIndex].ConfirmAudioClip)
            {
                audioSource.clip = MenuItems[selectedItemIndex].ConfirmAudioClip;
            }
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

        if (UpAction.triggered)
        {
            Select((index + count - 1) % count);
            inputDelayTimer = inputDelay;
        }
        else if (DownAction.triggered)
        {
            Select((index + count + 1) % count);
            inputDelayTimer = inputDelay;
        }
        else if (EnterAction.triggered)
        {
            Enter();
            inputDelayTimer = inputDelay;
        }
    }
}