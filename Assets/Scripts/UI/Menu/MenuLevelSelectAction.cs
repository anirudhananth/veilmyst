using System;
using Cinemachine;
using TMPro;
using UnityEngine;

[Serializable]
public struct LevelData
{
    public string LevelID;
    public string DisplayName;
    public Showable Grid;
}

public class MenuLevelSelectAction : MenuActionBase
{
    public LevelData[] Levels;
    public int SelectedLevelIndex = -1;
    public TextMeshProUGUI LevelName;
    public CinemachineVirtualCamera Camera;

    LevelData curLevel => Levels[SelectedLevelIndex];
    string levelText => $"{curLevel.DisplayName} ({curLevel.LevelID})";
    private AudioSource audioSource;

    public override void Start()
    {
        base.Start();
        Debug.Assert(Levels.Length > 0, "No levels configured!");
        menuItem.RegisterShow(HandleShow);
        audioSource = GetComponent<AudioSource>();
    }

    public override void Trigger(Menu source)
    {
        MainManager.LoadScene(curLevel.LevelID);
    }

    public override void Select(Menu sourceMenu)
    {
        Refresh();
    }

    public override void Deselect(Menu sourceMenu)
    {
        Refresh();
    }

    private void Refresh()
    {
        if (!menuItem?.ParentMenu) return;

        Camera.Priority = menuItem.ParentMenu.MenuCamera.Priority + (menuItem.Selected ? 1 : -1);
        SelectLevel(0);
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
            curLevel.Grid.SetShow(false);
        }

        SelectedLevelIndex = nextLevelIndex;
        LevelName.text = levelText;
        Levels[nextLevelIndex].Grid.SetShow(true);
    }

    private void Update()
    {
        if (!menuItem.ParentMenu || !menuItem.ParentMenu.HasFocus) return;

        var p = menuItem.ParentMenu;

        int diff = 0;
        if (p.LeftAction.triggered) diff = -1;
        else if (p.RightAction.triggered) diff = 1;
        if (diff != 0)
        {
            audioSource.Play();
            SelectLevel((SelectedLevelIndex + Levels.Length + diff) % Levels.Length);
        }
    }

    private void HandleShow(bool show)
    {
        Refresh();
    }
}