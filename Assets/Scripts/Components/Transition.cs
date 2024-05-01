using UnityEngine;

[RequireComponent(typeof(Showable))]
public class Transition : MonoBehaviour
{
    public void Awake()
    {
        MainManager.TransitionAnimator = GetComponent<Showable>();
    }
}