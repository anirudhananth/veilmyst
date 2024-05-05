using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public static Showable TransitionAnimator;
    public static bool InGame = false;
    public float MaxStamina = 100;
    public RebindSaveLoad RebindSaveLoad;
    public SavesManager SavesManager;
    public string CurrentLevel;
    public static LevelEvent LevelLoadEvent
    {
        get
        {
            if (m_LevelLoadEvent == null)
                m_LevelLoadEvent = new LevelEvent();
            return m_LevelLoadEvent;
        }
    }
    public static LevelEvent LevelEndEvent
    {
        get
        {
            if (m_LevelEndEvent == null)
                m_LevelEndEvent = new LevelEvent();
            return m_LevelEndEvent;
        }
    }
    private static LevelEvent m_LevelLoadEvent;
    private static LevelEvent m_LevelEndEvent;

    public static IEnumerator DelayedLoadScene(string scene)
    {
        InGame = false;
        if (TransitionAnimator)
        {
            TransitionAnimator.SetShow(true);
        }
        else
        {
            Debug.LogError("Missing Transition game object");
        }
        m_LevelEndEvent?.Invoke(Instance.CurrentLevel);
        yield return new WaitForSeconds(0.8f);
        Instance.CurrentLevel = scene;
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        do
        {
            yield return new WaitForSeconds(0.16f);
        } while (!op.isDone);
        InGame = !SavesManager.Instance.CurrentLevelStat.isMenu;
        m_LevelLoadEvent?.Invoke(scene);
    }

    public static void LoadScene(string scene)
    {
        Instance.StartCoroutine(DelayedLoadScene(scene));
    }

    private void Awake()
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
        CurrentLevel = SceneManager.GetActiveScene().name;
        SavesManager.Load();
        InGame = !SavesManager.CurrentLevelStat.isMenu;
    }

    private void Start()
    {
        RebindSaveLoad.Load();
    }

    [Serializable]
    public class LevelEvent : UnityEvent<string>
    {
    }
}
