
using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Samples.RebindUI;

public enum AnimationType
{
    Jump, Movement, Dash
}

public class MenuRebindAction : MenuHorizontalAction
{
    public TextMeshProUGUI ActionTextBox;
    public CinemachineVirtualCamera Camera;
    public AnimationType Animation;
    public Movement Player;

    private RebindActionUI rebindActionUI;
    private bool resetMode;
    private string actionText;
    private bool lockedIn;
    private bool goRight = true;

    private void Awake()
    {
        actionText = ActionTextBox.text;
    }

    public override void Start()
    {
        base.Start();
        rebindActionUI = GetComponent<RebindActionUI>();
        menuItem.RegisterShow(HandleShow);
        RegisterMove(HandleMove);
    }

    public override void Trigger(Menu source)
    {
        if (resetMode)
        {
            rebindActionUI.ResetToDefault();
        }
        else
        {
            rebindActionUI.StartInteractiveRebind();
            Camera.Priority = source.MenuCamera.Priority + 1;
            lockedIn = true;
            Player.AnimationOverride = true;
            Player.overiddenIsJumping = false;
            Player.overiddenMoveDelta = Vector2.zero;
            Player.overiddenIsDashing = false;
            switch (Animation)
            {
                case AnimationType.Jump:
                    Player.overiddenIsJumping = true;
                    break;
                case AnimationType.Movement:
                    Player.overiddenMoveDelta = goRight ? Vector2.right : Vector2.left;
                    goRight = !goRight;
                    break;
                case AnimationType.Dash:
                    Player.overiddenIsDashing = true;
                    Player.overiddenMoveDelta = goRight ? Vector2.right : Vector2.left;
                    break;
            }
        }
        Refresh();
    }

    public override void Deselect(Menu sourceMenu)
    {
        resetMode = false;
        Refresh();
    }

    private void HandleShow(bool show)
    {
        Refresh();
    }

    private void HandleMove(int diff)
    {
        resetMode = diff > 0;
        Refresh();
    }

    private void Refresh()
    {
        if (resetMode && menuItem.Selected) ActionTextBox.text = "Reset to Default";
        else ActionTextBox.text = actionText;

        Player.AnimationOverride = lockedIn;
        HintVariant variant = resetMode ? HintVariant.RebindReset : HintVariant.RebindNormal;
        if (menuItem?.ParentMenu)
        {
            menuItem.ParentMenu.SetHintText(variant);
            Camera.Priority = menuItem.ParentMenu.MenuCamera.Priority + (lockedIn ? 1 : -1);
        }
    }

    private void Update()
    {
        if (menuItem.ParentMenu == null || !menuItem.ParentMenu.HasFocus) return;

        if (lockedIn && rebindActionUI?.ongoingRebind == null)
        {
            lockedIn = false;
            rebindActionUI.rebindPrompt.text = "";
            Refresh();
        }
    }
}