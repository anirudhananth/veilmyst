using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public delegate void OnCollideDash(DashDamageTaker self);

[RequireComponent(typeof(Collider2D))]
public class DashDamageTaker : MonoBehaviour
{
    [SerializeField]
    public GameObject Parent;
    public OnCollideDash OnCollideDash = DefaultDashHandler;

    public static void DefaultDashHandler(DashDamageTaker self)
    {
        //destroy self prefab
        //If gameobject has script called Enemy

        Enemy enemy;
        if (self.Parent.TryGetComponent(out enemy))
        {
            enemy.OnDeath();
        }
        else
        {
            Destroy(self.gameObject);
        }

    }

    private void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        Debug.Assert(collider != null);
        Debug.Assert(collider.isTrigger, $"{gameObject} must set isTrigger on the collider to true");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Movement movement;
        if (other.gameObject.TryGetComponent(out movement))
        {
            if (movement.isDashing)
            {
                Enemy enemy;
                if ((Parent && Parent.TryGetComponent(out enemy)) || (!Parent && TryGetComponent(out enemy)))
                {
                    if (enemy.isDead)
                    {
                        OnCollideDash(this);
                        return;
                    }
                }
                movement.DashImpact((Parent) ? Parent : gameObject);
                OnCollideDash(this);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) 
    {
        Movement movement;
        if (other.gameObject.TryGetComponent(out movement))
        {
            if (movement.isDashing)
            {
                Enemy enemy;
                if ((Parent && Parent.TryGetComponent(out enemy)) || (!Parent && TryGetComponent(out enemy)))
                {
                    if (enemy.isDead)
                    {
                        OnCollideDash(this);
                        return;
                    }
                }
                movement.DashImpact((Parent) ? Parent : gameObject);
                OnCollideDash(this);
            }
        }
        
    }
}
