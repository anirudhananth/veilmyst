
using System;
using TMPro;
using UnityEngine;

public class MenuVolumeAction : MenuHorizontalAction
{
    public TextMeshProUGUI TextBox;

    int volume;
    const int maxVolume = 10;
    public float RealVolume => volume / (float)maxVolume;

    public override void Start()
    {
        base.Start();
        menuItem.RegisterShow(HandleShow);
        RegisterMove(HandleMove);
    }

    public override void Trigger(Menu source) { }

    public override void Select(Menu sourceMenu)
    {
        menuItem.ParentMenu.SetHintText(HintVariant.Volume);
    }

    public override void Deselect(Menu sourceMenu)
    {
        menuItem.ParentMenu.SetHintText(HintVariant.Default);
    }

    private void HandleShow(bool show)
    {
        volume = (int)(AudioListener.volume * maxVolume);
        Refresh();
        if (menuItem?.ParentMenu)
        {
            if (menuItem.Selected) menuItem.ParentMenu.SetHintText(HintVariant.Volume);
        }
    }

    private void HandleMove(int diff)
    {
        volume += diff;
        volume = Math.Clamp(volume, 0, maxVolume);
        AudioListener.volume = RealVolume;
        Refresh();
    }

    private void Refresh()
    {
        TextBox.text = new string('|', volume) + new string('·', maxVolume - volume);
    }
}