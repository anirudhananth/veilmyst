using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Enemy : Actor
{
    public bool isSpawned = false;
    public bool isDead = false;
    public EnemySpawner Spawner;
    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;

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

    protected void Die(float delay = 1f)
    {
        if (isSpawned)
        {
            Spawner.Spawn();
        }

        audioSource.Play();
        animator.SetBool("isDead", true);
        Destroy(gameObject, delay);
    }

    public virtual void OnDeath()
    {
        if (isDead)
        {
            return;
        }
        Disable();
        Die();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        animator.SetBool("isMoving", rb.velocity.magnitude > 0.01);
    }
}
