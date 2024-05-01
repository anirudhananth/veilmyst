using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerAudio))]
public class Movement : MonoBehaviour
{
    private Collision coll;
    [HideInInspector]
    public Rigidbody2D rb;

    private PlayerAudio playerAudio;
    private PlayerInput input;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction wallClimbAction;

    private AnimationScript anim;
    public Vector3 spawnLocation;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 10f;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool canWallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool canWallSlide = true;
    public bool canDash = true;
    public bool isDashing;
    public bool hasDashed;
    public bool isHoldingJump;
    public bool canJump;
    public bool isGroundDash;
    public bool isGroundDashing;
    public bool isGroundDashed;
    public bool isGroundDashWindow;
    public bool isWallClimbForce;

    [Space]

    private bool groundTouch;
    [NonSerialized]
    private float x, y;
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    public int side = 1;
    private bool isUpwardForce = true;
    private float wallSideForce;
    private float dashDelay = 50f; //Milliseconds
    private bool isDashPressed = true;
    private DateTime dashPressedTime;

    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

    [Space]
    [Header("Stamina")]
    public Stamina stamina;

    public bool AnimationOverride = false;
    public bool overiddenIsDashing = false;
    public bool overiddenIsJumping = false;
    public Vector2 overiddenMoveDelta;

    private bool animationOverride = false;
    private const float maxAnimationInterval = 1f;
    private float animationInterval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
        playerAudio = GetComponent<PlayerAudio>();
        rb.gravityScale = 3;
        spawnLocation = transform.position;

        isDashPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null)
        {
            input = GetComponent<PlayerInput>();
            moveAction = input.actions["Move"];
            jumpAction = input.actions["Jump"];
            dashAction = input.actions["Dash"];
            wallClimbAction = input.actions["WallClimb"];
        }
        Vector2 moveDelta = moveAction.ReadValue<Vector2>();
        if (AnimationOverride)
        {
            if (animationInterval > 0) {
                animationInterval -= Time.deltaTime;
            }
            else
            {
                animationOverride = true;
                animationInterval = maxAnimationInterval;
            }
        }

        if (AnimationOverride) moveDelta = overiddenMoveDelta;
        x = moveDelta.x;
        y = moveDelta.y;
        Vector2 dir = new Vector2(x, y);

        if (!(AnimationOverride && overiddenIsDashing))
        {
            Walk(moveDelta);
            anim.SetHorizontalMovement(x, y, rb.velocity.y);
        }

        if(isWallClimbForce) {
            rb.gravityScale = 0;
            if(isUpwardForce && coll.onWall) {
                if(coll.onPlatformWall && coll.platform) rb.velocity = new(rb.velocity.x, rb.velocity.y);
                rb.velocity += Vector2.up * 85 * Time.deltaTime;
            } else if(isUpwardForce && !coll.onWall) {
                rb.velocity += Vector2.up * 75 * Time.deltaTime;
                rb.velocity += wallSideForce * Vector2.right * 35 * Time.deltaTime;
            } else {
                if(!coll.onPlatformWall) rb.gravityScale = 3;
                // rb.velocity += wallSideForce * Vector2.right * 25 * Time.deltaTime;
                if(coll.onGround) {
                    isWallClimbForce = false;
                    rb.gravityScale = 3;
                    return;
                }
            }
            return;
        }

        if (canWallGrab && coll.onClimbableWall && wallClimbAction.ReadValue<float>() != 0 && canMove)
        {
            if(side != coll.wallSide)
                anim.Flip(side*-1);
            wallGrab = true;
            wallSlide = false;
        }

        if (wallClimbAction.ReadValue<float>() == 0 || !coll.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }
        
        if (wallGrab && !isDashing && coll.onClimbableWall && canMove)
        {
            rb.gravityScale = 0;

            if(x > .2f || x < -.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rb.velocity = new Vector2(0, y * (speed * speedModifier));
            // if(!coll.onPlatformWall) {
            //     // transform.position = new Vector3(transform.position.x, transform.position.y + y * (speed * speedModifier) * Time.deltaTime, transform.position.z);
            // } else if(coll.platform) {
            //     Rigidbody2D platform = coll.platform.GetComponent<Rigidbody2D>();
            //     Vector2 platformVelocity = platform.velocity;
            //     float wallSide = coll.onRightWall ? 1 : -1;
            //     rb.velocity = new Vector2(rb.velocity.x + wallSide, platformVelocity.y + y * (speed * speedModifier));
            // }
        }
        else if (!wallGrab && !isDashing)
        {
            if(!coll.riding) {
                rb.gravityScale = 3;
            }
        }

        if (y > 0 && wallGrab && ((coll.onLeftWall && !coll.onTopLeftWall) || (coll.onRightWall && !coll.onTopRightWall))) {
            StartCoroutine(WallClimbEndForce());
        }

        if(coll.onWall && !coll.onPlatformWall && !coll.onGround && !isDashing && rb.velocity.y < 0)
        {
            if (x != 0 && !wallGrab && (canWallSlide || rb.velocity.y < 0))
            {
                if(stamina!=null)
                {
                    if(stamina.currentstamina<=0)
                    {
                        wallSlide = false;
                        return;
                    }
                }

                wallSlide = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround) {
            wallSlide = false;
        }
        
        if (jumpAction.triggered || (animationOverride && overiddenIsJumping && coll.onGround))
        {
            isHoldingJump = true;
            anim.SetTrigger("jump");

            canWallSlide = true;
            if (coll.onWall && coll.onGround) {
                canWallSlide = false;
                StartCoroutine(SetWallSlide());
                Jump(Vector2.up, false);
                return;
            }
            if (!coll.onWall && coyoteTimeCounter > 0 && ((isDashing && isGroundDash) || !isDashing)) 
            {
                Jump(Vector2.up, false);
                return;
            }
            if (coll.onWall && !coll.onGround && ((isDashing && isGroundDash) || !isDashing))
            {    
                WallJump();
                return;
            }
        }

        if (Input.GetButtonUp("Jump")) {
            isHoldingJump = false;
            canJump = true;
        }

        if (!isDashPressed && canDash && (dashAction.triggered || (animationOverride && overiddenIsDashing)) && !hasDashed)
        {
            isDashPressed = true;
            dashPressedTime = DateTime.Now;
        }

        if (isDashPressed && (DateTime.Now - dashPressedTime).TotalMilliseconds > dashDelay)
        {
            isDashPressed = false;
            Dash(x, y);
        }

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if(!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if(coll.onGround) 
        {
            if(stamina!=null)
            {
                stamina.StartRecharge();
            }
            if(rb.velocity.y < 0 && !coll.onPlatform) {
                rb.velocity = new(rb.velocity.x, 0);
            }
        }
        else
        {
            if(stamina!=null)
            {
                stamina.StopRecharge();
            } 
        }

        WallParticle(y);

        // if(hasDashed && !isDashing && (coll.onGround || coll.onWall)) {
        //     hasDashed = false;
        // }

        if(!hasDashed && !isDashing && coll.onGround) {
            canDash = true;
        }

        if(isGroundDashed && (coll.onGround || coll.onWall)) {
            StartCoroutine(ResetDash());
            isGroundDashing = false;
            hasDashed = false;
            isDashing = false;
            isGroundDashed = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        if(coll.onGround) {
            coyoteTimeCounter = coyoteTime;
            isWallClimbForce = false;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }


        if (wallGrab || wallSlide || !canMove)
            return;

        if(!isDashing) {
            if(x > 0)
            {
                side = 1;
                anim.Flip(side);
            }
            if (x < 0)
            {
                side = -1;
                anim.Flip(side);
            }
        }

        if (animationOverride)
        {
            overiddenMoveDelta.x *= -1;
        }
        animationOverride = false;
    }

    IEnumerator ResetDash() {
        dashParticle.Stop();
        isGroundDashed = false;

        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        yield return new WaitForSeconds(0.3f);
        isGroundDashWindow = false;
    }

    IEnumerator SetWallSlide() {
        canWallSlide = false;
        yield return new WaitForSeconds(0.25f);
        canWallSlide = true;
    }

    IEnumerator SetWallGrab() {
        canWallSlide = false;
        canWallGrab = false;
        yield return new WaitForSeconds(0.25f);
        canWallGrab = true;
    }

    IEnumerator WallClimbEndForce() {
        isWallClimbForce = true;
        float wallSide = coll.onRightWall ? 1 : -1;
        wallSideForce = wallSide;
        isUpwardForce = true;

        yield return new WaitForSeconds(0.25f);

        // rb.velocity = Vector2.zero;

        isUpwardForce = false;
        // while (time < duration / 2f) {
        //     rb.AddForce(Vector2.up * 0.1f, ForceMode2D.Force);
        //     time += Time.deltaTime;
        //     yield return null;
        // }

        // time = 0;

        // while (time < duration / 2f) {
        //     rb.AddForce(wallSide * Vector2.right * 0.2f, ForceMode2D.Force);
        //     time += Time.deltaTime;
        //     yield return null;
        // }
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }

    public void DashImpact(GameObject other)
    {
        Vector3 dir = (transform.position - other.transform.position).normalized;
        dir.y += 0.5f;
        dir.Normalize();
        rb.velocity = dir * rb.velocity.magnitude * 0.8f;
        canDash = true;
    }

    private void Dash(float x, float y)
    {
        if(stamina!=null)
        {
            bool canDash = stamina.ReduceStamina(stamina.dash_stamina);
            if(canDash)
            {
                stamina.StopRecharge();
            }
            else
            {
                return;
            }
        }
        canDash = false;
        isDashing = true;
        hasDashed = true;
        GetComponent<BetterJumping>().enabled = false;
        // if(isHoldingJump) canJump = false;
        if(x != 0) {
            x = x < 0 ? -1f : 1f;
        }
        if(y != 0) {
            y = y < 0 ? -1f : 1f;
        }

        if(y == 0 && coll.onGround) {
            isGroundDash = true;
        } else {
            isGroundDash = false;
        }

        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        // ------------------------- IMPORTANT -------------------------
        // FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        // rb.velocity = Vector2.zero;

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);
        if (x == 0 && y == 0) {
            dir = new(side, 0);
        }
        if (dir.x != 0)
        {
            anim.SetTrigger("dashSide");
        }
        else
        {
            anim.SetTrigger(dir.y > 0 ? "dashUp" : "dashDown");
        }
        playerAudio.PlayDash();

        if (isGroundDash) {
            StartCoroutine(GroundDashWait(dir));
        } else {
            StartCoroutine(DashWait(dir));
        }
    }

    IEnumerator GroundDashWait(Vector2 dir) {
        isGroundDashing = true;
        isGroundDashWindow = true;
        StartCoroutine(GroundDash());
        rb.gravityScale = 3;
        rb.velocity = dir.normalized * dashSpeed;

        dashParticle.Play();
        
        yield return new WaitForSeconds(.3f);

        isGroundDashed = true;
        if(stamina!=null)
        {
            stamina.StartRecharge();
        }
        isGroundDash = false;
    }

    IEnumerator DashWait(Vector2 dir)
    {
        // ------------------------- IMPORTANT -------------------------
        // FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        // DOVirtual.Float(14, 0, .2f, RigidbodyDrag);

        rb.gravityScale = 0;
        rb.velocity = dir.normalized * dashSpeed;
        dashParticle.Play();
        wallJumped = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        rb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.35f);
        if (coll.onGround)
            hasDashed = false;
    }

    private void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        float _x = x;
        if(_x != 0) {
            _x = _x < 0 ? -1f : 1f;
        }
        if(_x == 0) {
            if(wallGrab) {
                Jump(Vector2.up / 1f, true);
                StartCoroutine(SetWallGrab());
            } else {
                Jump((Vector2.up / 1f + wallDir / 2f), true);
            }
        } else if(_x != 0) {
            if(_x == wallDir.x) {
                Jump((Vector2.up / 1f + wallDir / 2f), true);
            } else {
                Jump((Vector2.up / 1f + wallDir / 2.5f), true);
            }
            // Jump((Vector2.up / 1f + wallDir / 2f), true);
        }

        wallJumped = true;
    }

    private void WallSlide()
    {
        if(coll.wallSide != side)
         anim.Flip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = false;
        if((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        if(!coll.onPlatformWall) {
            rb.velocity = new Vector2(push, -slideSpeed);
        } else if(coll.platform) {
            Vector2 platformVelocity = coll.platform.GetComponent<Rigidbody2D>().velocity;
            rb.velocity = new Vector2(push + platformVelocity.x, -slideSpeed);
        }
        // rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void Walk(Vector2 dir)
    {
        if (isDashing)
            return;

        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!coll.onGround && isGroundDashWindow)
            return;

        if (coll.onGround && isGroundDashing) return;

        if(!wallJumped && isGroundDashing && !coll.onGround && !coll.onWall)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
        else if (!wallJumped)
        {
            if(isWallClimbForce) {
                rb.velocity = new Vector2(0, rb.velocity.y);
            } else if(!coll.riding) {
                rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
            } else {
                Vector2 platformVelocity = coll.riding.velocity;
                // rb.velocity = new Vector2(dir.x * speed + platformVelocity.x, rb.velocity.y);
                rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed + platformVelocity.x, rb.velocity.y)), 30f * Time.deltaTime);
            }
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
        if (Mathf.Abs(rb.velocity.x) > 0.5 && coll.onGround && (Mathf.Abs(x) == 1)) playerAudio.PlayWalk();
    }

    public void Jump(Vector2 dir, bool wall)
    {
        if(stamina!=null)
        {
            bool canDash = stamina.ReduceStamina(stamina.normaljump_stamina);
            if(canDash)
            {
                stamina.StopRecharge();
            }
            else
            {
                return;
            }
        }
        dir = dir.normalized;
        if(isDashing && isGroundDashing) {
            StartCoroutine(ResetDash());
        }

        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        if(!wall) rb.velocity = new Vector2(rb.velocity.x, 0);
        if(coll.riding) {
            rb.gravityScale = 3f;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(coll.riding.velocity.y, 0));
        } else if(coll.onPlatformWall && !coll.onGround && coll.platform) {
            Rigidbody2D platform = coll.platform.GetComponent<Rigidbody2D>();
            Vector2 platformVelocity = platform.velocity;
            rb.velocity = new Vector2(0, Mathf.Max(platformVelocity.y, 0));
        } else if(wall) rb.velocity = Vector2.zero;

        rb.velocity += dir * jumpForce;

        particle.Play();
        playerAudio.PlayJump();
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }
}
