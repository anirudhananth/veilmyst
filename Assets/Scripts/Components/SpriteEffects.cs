using System.Linq;
using UnityEngine;

public class SpriteEffects : MonoBehaviour
{
    public Animator Animator;

    public void Play(Vector2 pos, string animation, float duration = 2f)
    {
        transform.position = pos;
        Animator.SetTrigger(animation);
        Destroy(gameObject, duration);
    }
}