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
    private Movement movement;

    public bool isshortJump = false;

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
        
        isshortJump = false;
        if(rb.velocity.y < 0) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } 
        else if(rb.velocity.y > 0 && jumpAction.ReadValue<float>() == 0) 
        {
            //short jump triggered here?
            isshortJump = true;
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier  ) * Time.deltaTime;
        } 
        else if(rb.velocity.y > 0 && jumpAction.ReadValue<float>() != 0 && (movement.hasDashed && !movement.wallJumped)) 
        {
            //When is this triggered?
            Debug.Log("value not 0");
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
