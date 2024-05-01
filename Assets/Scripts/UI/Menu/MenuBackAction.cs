using UnityEngine;

public class MenuBackAction : MenuActionBase
{
    public override void Trigger(Menu source)
    {
        source.SetShow(false);
        source.ParentMenu.SetShow(true);
    }
}