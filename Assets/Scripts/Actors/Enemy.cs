using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public bool isSpawned = false;
    public bool isDead = false;
    public EnemySpawner Spawner;
    private Animator animator;
    private Rigidbody2D rb;

    public void OnDeath()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        animator.SetBool("isDead", true);
        Destroy(gameObject, 1f);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Patrolling>().IsPaused = true;
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
    }

    private void Update()
    {
        animator.SetBool("isMoving", rb.velocity.magnitude > 0.01);
    }
}
