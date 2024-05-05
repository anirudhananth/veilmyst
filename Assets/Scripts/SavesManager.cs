using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public struct LevelStat
{
    public string name;
    public bool isMenu;
    public bool unlocked;

    public int deathCount;
    public string[] collectedCrownsID;
    public string[] crownsID;
}

public struct GameSave
{
    public LevelStat[] levelStats;
}

public class SavesManager : MonoBehaviour, ISavable
{
    public static SavesManager Instance;
    [SerializeField]
    public Dictionary<string, int> levelStatsIndex
    {
        get;
        private set;
    }
    public UnityEvent StatsChanged = new();

    [SerializeField]
    public LevelStat[] m_LevelStats;

    // Computed fields
    public LevelStat CurrentLevelStat => m_LevelStats[levelStatsIndex[MainManager.Instance.CurrentLevel]];
    public int TotalDeaths => m_LevelStats.Aggregate(0, (acc, cur) => acc + cur.deathCount);
    public int TotalCrownsCollected => m_LevelStats.Aggregate(0, (acc, cur) => acc + cur.collectedCrownsID.Length);
    public int TotalCrowns => m_LevelStats.Aggregate(0, (acc, cur) => acc + cur.crownsID.Length);
    public int TotalLevels => m_LevelStats.Length;
    public int LevelsUnlocked => m_LevelStats.Count(l => l.unlocked);

    // Stats related to completion calculation
    public int crownCompletionWeight = 20;
    public int levelCompletionWeight = 100;
    public int MaxCompletionPoints => TotalCrowns * crownCompletionWeight + TotalLevels * levelCompletionWeight;
    public int CurrentCompletionPoints => TotalCrownsCollected * crownCompletionWeight + LevelsUnlocked * levelCompletionWeight;
    public float Completion => CurrentCompletionPoints / (float)MaxCompletionPoints;

    public void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UpdateMap();
        MainManager.LevelEndEvent.AddListener(OnLevelEnd);
    }

    public void Save()
    {
        GameSave save;
        save.levelStats = m_LevelStats;
        var saveJson = JsonUtility.ToJson(save, true);
        Debug.Log($"Saving: \n{saveJson}");
        PlayerPrefs.SetString("save", saveJson);
    }

    public void Load()
    {
        var saveJson = PlayerPrefs.GetString("save");
        return;
        Debug.Log($"Loading: \n{saveJson}");
        if (!string.IsNullOrEmpty(saveJson))
        {
            GameSave save = JsonUtility.FromJson<GameSave>(saveJson);
            m_LevelStats = save.levelStats;
            UpdateMap();
        }
    }

    public void StatAddDeath()
    {
        var stat = CurrentLevelStat;
        stat.deathCount++;
        SaveStat(stat);
        StatsChanged?.Invoke();
    }

    public void StatCollectCrown(string ID)
    {
        var stat = CurrentLevelStat;
        Debug.Assert(!stat.collectedCrownsID.Contains(ID), $"Double collecting a collected crown! (ID={ID})");
        stat.collectedCrownsID = stat.collectedCrownsID.Append(ID).ToArray();
        SaveStat(stat);
        StatsChanged?.Invoke();
    }

    private void SaveStat(LevelStat stat)
    {
        m_LevelStats[levelStatsIndex[MainManager.Instance.CurrentLevel]] = stat;
    }

    private void UpdateMap()
    {
        levelStatsIndex = new();
        for (int index = 0; index < m_LevelStats.Length; index++)
        {
            levelStatsIndex[m_LevelStats[index].name] = index;
        }
    }

    private void OnLevelEnd(string level)
    {
        // Trigger an autosave whenever a level ends (including the main menu)
        Save();
    }
}