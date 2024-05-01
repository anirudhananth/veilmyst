using UnityEngine;

public class MenuStartAction : MenuActionBase
{
    public override void Trigger()
    {
        MainManager.LoadScene("0-1");
    }
}