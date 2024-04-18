using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Transition : MonoBehaviour
{
    public void Start()
    {
        MainManager.TransitionAnimator = GetComponent<Animator>();
    }
}