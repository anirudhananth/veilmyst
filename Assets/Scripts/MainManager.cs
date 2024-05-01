using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public static Showable TransitionAnimator;
    public float MaxStamina = 100;

    public static IEnumerator DelayedLoadScene(string scene)
    {
        if (TransitionAnimator)
        {
            TransitionAnimator.SetShow(true);
        }
        else
        {
            Debug.LogError("Missing Transition game object");
        }
        yield return new WaitForSeconds(0.8f);
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        do {
            yield return new WaitForSeconds(0.16f);
        } while (!op.isDone);
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
    }
}
