
using System;
using TMPro;
using UnityEngine;

public class MenuToggleMusicAction : MenuActionBase
{
    public TextMeshProUGUI TextBox;
    private bool musicOn = true;

    public override void Trigger(Menu source)
    {
        musicOn = !musicOn;
        MainManager.Instance.GetComponent<AudioSource>().mute = !musicOn;
        TextBox.text = musicOn ? "[x]" : "[ ]";
    }
}