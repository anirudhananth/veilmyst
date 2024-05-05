using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{

    public AnimatedText deathTextbox;
    public AnimatedText collectedCrownsTextbox;
    public AnimatedText crownsTextbox;

    private int deathCount => MainManager.Instance.SavesManager.CurrentLevelStat.deathCount;
    private int collectedCrownsCount => MainManager.Instance.SavesManager.CurrentLevelStat.collectedCrownsID.Length;
    private int crownsCount => MainManager.Instance.SavesManager.CurrentLevelStat.crownsID.Length;

    private void Start()
    {
        MainManager.Instance.SavesManager.StatsChanged.AddListener(RedrawStats);
        RedrawStats();
    }

    private void RedrawStats()
    {
        if (deathTextbox) deathTextbox.text = deathCount.ToString();
        if (collectedCrownsTextbox) collectedCrownsTextbox.text = collectedCrownsCount.ToString();
        if (crownsTextbox) crownsTextbox.text = crownsCount.ToString();
    }
}