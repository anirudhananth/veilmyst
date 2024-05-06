using System;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

[Serializable]
public struct LevelData
{
    public string LevelID;
    public string DisplayName;
    public Sprite PreviewImage;
}

public class MenuLevelSelectAction : MenuHorizontalAction
{
    public LevelData[] Levels;
    public int SelectedLevelIndex = -1;
    public TextMeshProUGUI LevelName;
    public TextMeshProUGUI LevelIndicator;
    public CinemachineVirtualCamera Camera;
    public GameObject PreviewPrefab;
    public Transform PreviewLocation;
    public StatsDisplay StatsDisplay;

    public Showable LockedText;
    public Showable CompletedText;

    private Dictionary<string, LevelPreview> previews;
    private LevelPreview curPreview
    {
        get
        {
            if (previews == null) InitPreviews();
            return previews[curLevel.LevelID];
        }
    }

    LevelData curLevel => Levels[SelectedLevelIndex];
    public LevelStat curLevelStat => SavesManager.Instance.GetLevelStat(curLevel.LevelID);
    string levelText => $"{curLevel.DisplayName} ({curLevel.LevelID})";

    public override void Start()
    {
        base.Start();
        Debug.Assert(Levels.Length > 0, "No levels configured!");
        menuItem.RegisterShow(HandleShow);

        RegisterMove(HandleMove);
    }

    public override void Trigger(Menu source)
    {
        if (curLevelStat.unlocked) MainManager.LoadScene(curLevel.LevelID, false);
    }

    public override void Select(Menu sourceMenu)
    {
        Refresh();
    }

    public override void Deselect(Menu sourceMenu)
    {
        Refresh();
    }

    private void InitPreviews()
    {
        previews = new();
        foreach (LevelData data in Levels)
        {
            LevelPreview preview = Instantiate(PreviewPrefab, PreviewLocation.position, PreviewLocation.rotation).GetComponent<LevelPreview>();
            preview.Init(data.LevelID, StatsDisplay, data.PreviewImage, LockedText, CompletedText);
            previews[data.LevelID] = preview;
        }
    }

    private void Refresh()
    {
        if (!menuItem?.ParentMenu) return;

        Camera.Priority = menuItem.ParentMenu.MenuCamera.Priority + (menuItem.Selected && menuItem.ParentMenu.HasFocus ? 1 : -1);
        SelectLevel(SelectedLevelIndex);
        if (menuItem.Selected)
        {
            menuItem.ParentMenu.SetHintText(HintVariant.Level);
            
            if (SelectedLevelIndex != -1) LevelName.text = levelText;
        }
        else
        {
            menuItem.ParentMenu.SetHintText(HintVariant.Default);
            LevelName.text = "";
        }
    }

    private void SelectLevel(int nextLevelIndex)
    {
        if (SelectedLevelIndex != -1)
        {
            curPreview.SetShow(false);
        }

        SelectedLevelIndex = nextLevelIndex;
        LevelIndicator.text = new string('|', SelectedLevelIndex) + 'x' + new string('|', previews.Count - SelectedLevelIndex - 1);
        LevelName.text = levelText;
        previews[Levels[nextLevelIndex].LevelID].SetShow(true);
    }

    private void HandleMove(int diff)
    {
        SelectLevel((SelectedLevelIndex + Levels.Length + diff) % Levels.Length);
    }

    private void HandleShow(bool show)
    {
        Refresh();
    }
}