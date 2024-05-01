
using System;
using TMPro;
using UnityEngine;

public class MenuToggleMusicAction : MenuActionBase
{
    public TextMeshProUGUI TextBox;
    private bool musicOn = true;

    public override void Start()
    {
        base.Start();
        menuItem.RegisterShow(HandleShow);
    }

    public override void Trigger(Menu source)
    {
        musicOn = !musicOn;
        MainManager.Instance.GetComponent<AudioSource>().mute = !musicOn;
        TextBox.text = musicOn ? "[x]" : "[ ]";
    }

    private void HandleShow(bool show)
    {
        if (show) Refresh();
    }

    private void Refresh()
    {
        musicOn = !MainManager.Instance.GetComponent<AudioSource>().mute;
        TextBox.text = musicOn ? "[x]" : "[ ]";
    }
}