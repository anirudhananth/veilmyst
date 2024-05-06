using UnityEngine;

public class MenuResetAction : MenuActionBase
{
    public override void Trigger(Menu source)
    {
        MainManager.Instance.SavesManager.ResetSave();
    }
}