using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Patrolling : MonoBehaviour
{
    [SerializeField]
    public GameObject pointA;

    [SerializeField]
    public GameObject pointB;

    [SerializeField]
    float speed = 1f;

    public bool IsPaused = false;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private Vector2 moveDirection;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        moveDirection = (currentPoint.position - transform.position).normalized;
        Debug.Assert(pointA != null);
        Debug.Assert(pointB != null);
    }

    void Update()
    {
        if (IsPaused) return;
        rb.velocity = moveDirection * speed;

        if(Vector2.Distance(transform.position, currentPoint.position)<0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
            moveDirection = (currentPoint.position - transform.position).normalized;
        }

        if(Vector2.Distance(transform.position, currentPoint.position)<0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
            moveDirection = (currentPoint.position - transform.position).normalized;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(pointA.transform.position,0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position,0.5f);
        Gizmos.DrawLine(pointA.transform.position,pointB.transform.position);
    }
}
