using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnCollideDestructible(LethalCollision self, Destructible destructible);

[RequireComponent(typeof(Collider2D))]
public class LethalCollision : MonoBehaviour
{
    public OnCollideDestructible OnCollideDestructible = DefaultCollisionHandler;

    public static void DefaultCollisionHandler(LethalCollision self, Destructible destructible)
    {
        destructible.Die(self.gameObject);
    }

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
            OnCollideDestructible(this, destructible);
        }
    }
}
