using DG.Tweening;
using UnityEngine;

public class MenuLevelSelectAction : MenuActionBase
{
    public Camera FromCamera;
    public Camera TargetCamera;
    public Menu TargetMenu;

    private Menu previousMenu;

    public override void Trigger(Menu source)
    {
       previousMenu = source; 
       previousMenu.HasFocus = false;
       TargetMenu.HasFocus = true;
    }
}