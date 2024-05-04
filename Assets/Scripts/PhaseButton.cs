using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseButton : MonoBehaviour
{
    public float cooldown = 0;
    public Animator AnimatorA;
    public Animator AnimatorB;
    public Animator AnimatorRay;

    private bool curPhase = false;
    private PhaseManager PM => GameManager.Instance.PhaseManager;

    private void SetActivation(bool active)
    {
        if (AnimatorA) AnimatorA.SetBool("active", active);
        if (AnimatorB) AnimatorB.SetBool("active", active);
        if (AnimatorRay) AnimatorRay.SetBool("active", active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if (cooldown < Time.time)
            {
                PM.PhaseChanger();
                SetActivation(true);
                cooldown = Time.time + .5f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SetActivation(false);
        }
    }

    private void Update()
    {
        if (curPhase != PM.CurrentPhase)
        {
            curPhase = PM.CurrentPhase;
            if (AnimatorA) AnimatorA.SetBool("phase", curPhase);
            if (AnimatorB) AnimatorB.SetBool("phase", curPhase);
        }

    }
}
