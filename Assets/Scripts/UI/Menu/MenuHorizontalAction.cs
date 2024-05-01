using System;
using Cinemachine;
using TMPro;
using UnityEngine;

public delegate void MoveHandler(int diff);

public abstract class MenuHorizontalAction : MenuActionBase
{
    private AudioSource audioSource;

    public override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    public event MoveHandler OnMove;

    public void RegisterMove(MoveHandler handler)
    {
        OnMove += handler;
    }

    private void Update()
    {
        if (!menuItem.ParentMenu || !menuItem.ParentMenu.HasFocus) return;

        var p = menuItem.ParentMenu;

        int diff = 0;
        if (p.LeftAction.triggered) diff = -1;
        else if (p.RightAction.triggered) diff = 1;
        if (diff != 0)
        {
            audioSource.Play();
            OnMove?.Invoke(diff);
        }
    }
}