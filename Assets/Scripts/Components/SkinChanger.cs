using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController AltAnimatorController;
    private RuntimeAnimatorController origAnimator;

    public void Start()
    {
        origAnimator = animator.runtimeAnimatorController;
        SavesManager.Instance.StatsChanged.AddListener(Refresh);
        Refresh();
    }

    private void Refresh()
    {
        if (Mathf.Approximately(SavesManager.Instance.Completion, 1))
        {
            animator.runtimeAnimatorController = AltAnimatorController;
        }
        else
        {
            animator.runtimeAnimatorController = origAnimator;
        }
    }
}