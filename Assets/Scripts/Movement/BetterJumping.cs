using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BetterJumping : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputAction jumpAction;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float maxFallSpeed = 8f;
    private Movement movement;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        
        if (jumpAction == null) 
        { 
            PlayerInput input = GetComponent<PlayerInput>();
            jumpAction = input.actions["Jump"];
        }
        if(rb.velocity.y < 0) 
        {
            //falling?
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } 
        else if(rb.velocity.y > 0 && jumpAction.ReadValue<float>() == 0) 
        {
            //short jump triggered here
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier-1) * Time.deltaTime;
        } 
        else if(rb.velocity.y > 0 && jumpAction.ReadValue<float>() != 0 && (movement.hasDashed && !movement.wallJumped)) 
        {
            //When is this triggered?
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        Vector2 vel = rb.velocity;
        vel.y = Mathf.Max(vel.y, -maxFallSpeed);
        rb.velocity = vel;
    }
}
