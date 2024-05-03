using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Enemy : Actor
{
    public bool isSpawned = false;
    public bool isDead = false;

    public bool useCustomizeDeathTime = false;
    public float deathanimtaitontime = 1f;
    public EnemySpawner Spawner;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected AudioSource audioSource;


    protected void Disable()
    {
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        var lc = GetComponentInChildren<LethalCollision>();
        if (lc) lc.enabled = false;
        var dash = GetComponentInChildren<DashDamageTaker>();
        if (dash) dash.enabled = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        Patrolling p;
        if (TryGetComponent<Patrolling>(out p))
        {
            GetComponent<Patrolling>().IsPaused = true;
        }
    }

    protected void Die(float death = 1f)
    {
        if (isSpawned)
        {
            Spawner.Spawn();
        }
        audioSource.Play();
        animator.SetBool("isDead", true);
        if (useCustomizeDeathTime)
        {
            Destroy(gameObject, deathanimtaitontime);
        }
        else
        {
            Destroy(gameObject, death);
        }
    }

    public virtual void OnDeath()
    {
        if (isDead)
        {
            return;
        }
        Disable();
        Die();
        StartCoroutine(FindObjectOfType<Movement>().ResetDash());
    }

    protected void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    protected void Update()
    {
        animator.SetBool("isMoving", rb.velocity.magnitude > 0.01);
    }
}
