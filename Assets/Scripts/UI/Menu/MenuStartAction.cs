using UnityEngine;

public class MenuStartAction : MenuActionBase
{
    public string sceneName = "0-1";

    public override void Trigger(Menu source)
    {
        MainManager.LoadScene(sceneName, false);
    }
}