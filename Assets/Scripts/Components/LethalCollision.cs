using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void OnCollideDestructible(LethalCollision self, Destructible destructible);

[RequireComponent(typeof(Collider2D))]
public class LethalCollision : MonoBehaviour
{
    [SerializeField]
    public Enemy Parent;
    public OnCollideDestructible OnCollideDestructible = DefaultCollisionHandler;

    public static void DefaultCollisionHandler(LethalCollision self, Destructible destructible)
    {
        destructible.Die((self.Parent) ? self.Parent.gameObject : self.gameObject);
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
        if (Parent && Parent.isDead) return;
        if (other.gameObject.TryGetComponent(out destructible))
        {
            OnCollideDestructible(this, destructible);
        }
    }
}
