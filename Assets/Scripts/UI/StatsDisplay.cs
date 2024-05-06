using Unity.VisualScripting;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{

    public AnimatedText deathTextbox;
    public AnimatedText collectedCrownsTextbox;
    public AnimatedText crownsTextbox;
    public AnimatedText completionTextbox;

    private SavesManager saves => MainManager.Instance.SavesManager;
    private LevelStat curStats => saves.CurrentLevelStat;

    public void DrawStats(LevelStat stat)
    {
        if (deathTextbox) deathTextbox.text = stat.deathCount.ToString();
        if (collectedCrownsTextbox) collectedCrownsTextbox.text = stat.collectedCrownsID.Length.ToString();
        if (crownsTextbox && stat.crownsID != null) crownsTextbox.text = stat.crownsID.Length.ToString();
        if (completionTextbox) completionTextbox.text = string.Format("{0:P2}", saves.Completion);
    }

    private void Start()
    {
        MainManager.Instance.SavesManager.StatsChanged.AddListener(RedrawStats);
        RedrawStats();
    }

    private void RedrawStats()
    {
        DrawStats(curStats);
    }
}