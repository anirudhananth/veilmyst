using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeathCallback(Destructible self, GameObject killer);
public delegate void SquishCallback(Destructible self, GameObject squisherA, GameObject squisherB, bool isHorizontal);

public class Destructible : MonoBehaviour
{
    public const float squishCheckRadius = 0.2f;
    public static LayerMask solidMask;
    public event DeathCallback DeathEvent;

    [Tooltip("To overide the death behavior, use GetComponent<Destructible> " +
             "and change OnDeath to the desired custom death handler.")]
    public DeathCallback OnDeath = new(OnDeathDefault);
    [Tooltip("To overide the squish behavior, use GetComponent<Destructible> " +
             "and change OnSquish to the desired custom squish handler.")]
    public SquishCallback OnSquish = new(OnSquishDefault);

    [Tooltip("Not actually a collider for physics. We use this as a marker " +
             "to determine when this destructible can be squished.")]
    public Collider2D squishBox;

    public static void OnDeathDefault(Destructible self, GameObject killer)
    {
        Destroy(self);
    }

    public static void OnSquishDefault(Destructible self, GameObject squisherA, GameObject squisherB, bool isHorizontal)
    {
        self.GetComponent<Rigidbody2D>().isKinematic = true;
        self.Die(squisherA);
    }

    public void Die(GameObject killer)
    {
        OnDeath(this, killer);
        DeathEvent?.Invoke(this, killer);
    }

    private bool CheckSquish(Collider2D left, Collider2D right, bool checkHorizontal)
    {
        if (left == null || right == null) return false;

        if (!left.CompareTag("Solid") || !right.CompareTag("Solid")) return false;

        // When two colliders are moving against each other
        Rigidbody2D rbA = left.GetComponent<Rigidbody2D>();
        Rigidbody2D rbB = right.GetComponent<Rigidbody2D>();
        Vector2 velA = (rbA) ? rbA.velocity : Vector2.zero;
        Vector2 velB = (rbB) ? rbB.velocity : Vector2.zero;

        float speedLeft, speedRight;
        // Separate horizontal and vertical checks
        if (checkHorizontal)
        {
            speedLeft = velA.x;
            speedRight = velB.x;
        }
        else
        {
            speedLeft = velA.y;
            speedRight = velB.y;
        }

        // At least one of them should be moving
        if (speedLeft == 0 && speedRight == 0) return false;

        return speedLeft >= 0 && speedRight <= 0;
    }

    private void FixedUpdate()
    {
        // Ground is a form of Solid
        if (solidMask == 0) solidMask = LayerMask.GetMask(new string[] { "Solid", "Ground" });
        Vector2 pos = transform.position;
        Vector2 center = squishBox.bounds.center;
        Vector2 extent = squishBox.bounds.extents;
        foreach (Collider2D left in Physics2D.OverlapCircleAll(center - extent * Vector2.right, squishCheckRadius, solidMask))
        {
            foreach(Collider2D right in Physics2D.OverlapCircleAll(center + extent * Vector2.right, squishCheckRadius, solidMask))
            {
                if (CheckSquish(left, right, true))
                {
                    OnSquish(this, left.gameObject, right.gameObject, true);
                    return;
                }
            }
        }

        foreach (Collider2D bot in Physics2D.OverlapCircleAll(center - extent * Vector2.up, squishCheckRadius, solidMask))
        {
            foreach (Collider2D top in Physics2D.OverlapCircleAll(center + extent * Vector2.up, squishCheckRadius, solidMask))
            {
                if (CheckSquish(bot, top, true))
                {
                    OnSquish(this, bot.gameObject, top.gameObject, true);
                    return;
                }
            }
        }
    }
}
