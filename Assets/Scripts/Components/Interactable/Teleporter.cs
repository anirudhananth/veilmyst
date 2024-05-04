using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject Target;
    public Animator SourceAnimator;
    public Animator DestAnimator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            SourceAnimator.SetTrigger("teleport");
            DestAnimator.SetTrigger("teleport");
            TriggerBehavior(other.gameObject);
        }
    }

    private void TriggerBehavior(GameObject gameObject)
    {
        Teleport(gameObject);
    }

    private void Teleport(GameObject gameObject)
    {
        gameObject.transform.SetPositionAndRotation(Target.transform.position, Quaternion.identity);
    }
}
