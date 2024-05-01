using UnityEngine;

public class SubMenuAction : MenuActionBase
{
    public Menu TargetMenu;

    public override void Trigger(Menu source)
    {
        source.SetShow(false);
        TargetMenu.SetShow(true);
        TargetMenu.ParentMenu = source;
    }
}