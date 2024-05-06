using UnityEngine;

public class MenuCheatAction : MenuActionBase
{
    public override void Trigger(Menu source)
    {
        MainManager.Instance.SavesManager.UnlockAll();
    }
}