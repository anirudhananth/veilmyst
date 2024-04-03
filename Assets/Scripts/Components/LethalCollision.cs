using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collision2D))]
public class LethalCollision : MonoBehaviour
{
    private void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        Debug.Assert(collider != null);
        Debug.Assert(collider.isTrigger, $"{gameObject} must set isTrigger on the collider to true");
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Destructible destructible;
        if (other.gameObject.TryGetComponent(out destructible))
        {
            destructible.Die(gameObject);
        }
    }
}
