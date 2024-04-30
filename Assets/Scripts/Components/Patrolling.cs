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
            if(Vector2.Distance(transform.position, currentPoint.position)<0.5f && currentPoint == pointB.transform)//reach point B
            {
                currentPoint = pointA.transform;
                moveDirection = (currentPoint.position - transform.position).normalized;
            }
            if(Vector2.Distance(transform.position, currentPoint.position)<0.5f && currentPoint == pointA.transform)//reach point A
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

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player"))
        {
            // other.transform.SetParent(transform);
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 50f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Player"))
        {
            // other.transform.SetParent(transform);
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3f;
        }
    }
}
