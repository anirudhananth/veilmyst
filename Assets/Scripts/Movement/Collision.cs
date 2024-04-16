using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask climbableWallLayer;

    [Space]

    [Header("Ground")]
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onTopRightWall;
    public bool onLeftWall;
    public bool onTopLeftWall;
    public int wallSide;

    [Space]

    [Header("Platform")]

    public Collider2D platform;
    public bool onPlatform;
    public bool onPlatformWall;
    public bool onPlatformRightWall;
    public bool onPlatformLeftWall;
    public bool onPlatformTopRightWall;
    public bool onPlatformTopLeftWall;
    public Rigidbody2D riding;

    [Space] 

    [Header("Platform")]

    public bool onClimbableWall;
    public bool onClimbableLeftWall;
    public bool onClimbableRightWall;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset, topLeftOffset, topRightOffset;
    private Color debugCollisionColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D groundCol = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        Collider2D platformCol = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, platformLayer);
        SetGroundBooleans(groundCol);
        SetPlatformBooleans(platformCol);
        SetClimbableWallBooleans();
    }

    void SetGroundBooleans(Collider2D groundCol) {
        onGround = groundCol;
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) 
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onTopRightWall = Physics2D.OverlapCircle((Vector2)transform.position + topRightOffset, collisionRadius, groundLayer);
        onTopLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + topLeftOffset, collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;
    }

    void SetPlatformBooleans(Collider2D platformCol) {
        onPlatform = platformCol;
        riding = (platformCol) ? platformCol.GetComponent<Rigidbody2D>() : null;

        onPlatformWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, platformLayer) 
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, platformLayer);
        onPlatformLeftWall = onLeftWall;
        onPlatformRightWall = onRightWall;
        onPlatformTopRightWall = onTopRightWall;
        onPlatformLeftWall = onTopLeftWall;

        if(onPlatformLeftWall) {
            platform = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, platformLayer);
        } else {
            platform = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, platformLayer);
        }
    }

    void SetClimbableWallBooleans() {
        onClimbableWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, climbableWallLayer) 
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, climbableWallLayer);

        onClimbableRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, climbableWallLayer);
        onClimbableLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, climbableWallLayer);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + topLeftOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + topRightOffset, collisionRadius);
    }
}
