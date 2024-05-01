using Cinemachine;
using UnityEngine;

public class SubMenuAction : MenuActionBase
{
    public CinemachineVirtualCamera FromCamera;
    public CinemachineVirtualCamera TargetCamera;
    public Menu TargetMenu;

    public override void Trigger(Menu source)
    {
        source.SetShow(false);
        TargetMenu.SetShow(true);
        TargetMenu.ParentMenu = source;
    }
}