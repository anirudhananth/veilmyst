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

    public void OnDeath()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        audioSource.Play();
        animator.SetBool("isDead", true);
        Destroy(gameObject, 1f);
        GetComponent<Collider2D>().enabled = false;
        Patrolling p;
        if(TryGetComponent<Patrolling>(out p))
        {
            GetComponent<Patrolling>().IsPaused = true;
        }
        GetComponentInChildren<LethalCollision>().enabled = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        if (isSpawned)
        {
            Spawner.Spawn();
        }
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
