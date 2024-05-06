using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("Main Variables")]
    public GameManager GM; // game manager
    public Vector3 CamPos;
    public Movement playerMovement;

    [Space]

    [Header("Set If Spawnpoint")]
    public bool isSpawnPoint = false;

    private Animator animator;
    private bool unlocked = false;
    private bool active = false;

    protected void Activate()
    {
        if (!unlocked) return;
        active = true;
        if (animator != null) animator.SetBool("active", true);
    }

    protected void Deactivate()
    {
        if (!unlocked) return;
        active = false;
        if (animator != null) animator.SetBool("active", false);
    }

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.keepAnimatorStateOnDisable = true;
        if (!isSpawnPoint) animator.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isSpawnPoint)
            {
                if (!unlocked)
                {
                    unlocked = true;
                    if (animator != null) animator.SetBool("unlocked", true);
                }
                if (!active)
                {
                    // There can only be one activate spawn point at a time.
                    if(GM.ActiveSpawnPoint)
                    {
                        GM.ActiveSpawnPoint?.Deactivate();
                    }
                    Activate();
                    
                }
                GM.SetSpawn(this);
            }
            GM.CPPos = transform.position;
            GM.MoveCam(CamPos);
        }
    }
}
