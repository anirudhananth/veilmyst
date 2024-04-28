using System;
using UnityEngine;

[RequireComponent(typeof(LethalCollision))]
public class Spike : MonoBehaviour
{
    private LethalCollision collision;

    [Tooltip("Only objects moving against this direction can be killed by the spike")]
    private Vector2 direction = Vector2.up;

    private void Start()
    {
        collision = GetComponent<LethalCollision>();
        direction.Normalize();

        void handler(LethalCollision self, Destructible destructible)
        {
            Rigidbody2D rb = destructible.GetComponent<Rigidbody2D>();

            // Only kill if the movement is in an opposite direction
            if (rb == null || Vector2.Dot(rb.velocity.normalized, direction) < 0.001)
            {
                destructible.Die(self.gameObject);
            }
        }

        collision.OnCollideDestructible = handler;
    }
}