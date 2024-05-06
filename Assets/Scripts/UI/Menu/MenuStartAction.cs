using UnityEngine;

public class MenuStartAction : MenuActionBase
{
    public override void Trigger(Menu source)
    {
        MainManager.LoadScene("0-1", false);
    }
}