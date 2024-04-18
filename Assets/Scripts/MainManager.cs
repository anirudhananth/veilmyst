using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public static Animator TransitionAnimator;
    public float MaxStamina=100;
    public static IEnumerator DelayedLoadScene(string scene)
    {
        if (TransitionAnimator)
        {
            TransitionAnimator.SetBool("shown", true);
        }
        else
        {
            Debug.LogError("Missing Transition game object");
        }
        yield return new WaitForSeconds(0.16f);
        SceneManager.LoadScene(scene);
    }

    public static void LoadScene(string scene)
    {
        Instance.StartCoroutine(DelayedLoadScene(scene));
    }

    private void Awake()
    {
        if(Instance ==null)
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
    public void IncreaseStamina(float amount)
    {
        MaxStamina+=amount;
    }
}
