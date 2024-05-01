using UnityEngine;

public class MenuBackAction : MenuActionBase
{
    public override void Trigger(Menu source)
    {
        source.SetShow(false);
        // Go back to the parent menu if we can
        if (source.ParentMenu)
        {
            source.ParentMenu.SetShow(true);
        }
    }
}