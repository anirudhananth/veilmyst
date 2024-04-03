using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpriteFlipper : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (rb.velocity.x > 0)
        {
            transform.localScale = originalScale;
        }
        else
        {
            transform.localScale = new Vector2(-originalScale.x, originalScale.y);
        }
    }
}
