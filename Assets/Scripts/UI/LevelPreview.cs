using UnityEngine;


// Display the death count, crowns collected, completion of the level.
public class LevelPreview : AnimatedUI
{
    public SpriteRenderer PreviewImage;
    public Color ColorLocked;
    public Color ColorNormal;

    private string levelID;
    private StatsDisplay statsDisplay;
    private Showable lockOverlay;
    private Showable completedOverlay;

    LevelStat levelStat => SavesManager.Instance.GetLevelStat(levelID);

    public void Init(string levelID, StatsDisplay statsDisplay, Sprite image, Showable locked, Showable complete)
    {
        this.levelID = levelID;
        this.statsDisplay = statsDisplay;
        PreviewImage.sprite = image;
        lockOverlay = locked;
        completedOverlay = complete;
    }

    public override void SetShow(bool show)
    {
        base.SetShow(show);

        if (show)
        {
            PreviewImage.color = levelStat.unlocked ? ColorNormal : ColorLocked;
            if (lockOverlay) lockOverlay.SetShow(!levelStat.unlocked);
            if (completedOverlay) completedOverlay.SetShow(levelStat.completed);
            statsDisplay.DrawStats(levelStat);
        }
    }
}