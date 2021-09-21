using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysics : MonoBehaviour
{
    #region Attributes
    [Header("Attributes")]
    public float startingMoveSpeed;
    public float startingJumpDelay;
    public float jumpForce;
    
    public float wallJumpForce;  

    [Header("Unity Components")]
    public float smoothFallMultiplier;
    public float groundRadius;
    public LayerMask groundLayer;
    public float groundCheckVerticalOffset;

    protected bool isFacingRight = true;
    protected bool wallJumpAvailable;
    protected bool onWall;
    protected float currentMoveSpeed;
    protected float currentJumpDelay;
    protected Rigidbody2D rb2d;
    protected FixedJoint2D joint2D;   

    private GameObject tileIgnoredForCollision;
    private float minDistanceToIgnoreCol = .5f;

    private float movementAxis;
    private float jumpAxis;
    #endregion

    #region Main Methods
    protected virtual void Awake() //ASSIGN COMPONENTS
    { 
        rb2d = GetComponent<Rigidbody2D>();
        joint2D = GetComponent<FixedJoint2D>();
        currentMoveSpeed = startingMoveSpeed;
    }

    protected virtual void FixedUpdate() // APPLY PHYSICS
    {
        //PHYSICS
        currentJumpDelay = currentJumpDelay < 0 ? currentJumpDelay = 0 : currentJumpDelay -= 1 * Time.deltaTime;
        HorizontalMovement(movementAxis);
        Jump(jumpAxis);
        SmoothFall();
        Flip();
        WallJump();
        ReEnableCollision();
        IsGrounded();
    }
    #endregion

    #region Methods
    protected bool IsGrounded() // CHECKS IF THE PLAYER IS TOUCHING THE GROUND
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y + groundCheckVerticalOffset);
        Vector2 size = new Vector2(groundRadius, groundRadius);
        return Physics2D.OverlapBox(position, size, 0, groundLayer);
    }

    private void HorizontalMovement(float direction) // MAKES THE PLAYER MOVE
    {
        if(!onWall)
        {
            currentMoveSpeed += 5 * Time.deltaTime;
            currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, 0.5f, startingMoveSpeed);

            rb2d.velocity = new Vector2(direction * currentMoveSpeed, rb2d.velocity.y);
        }
    }

    private void Jump(float jumpInput) // MAKES THE PLAYER JUMP 
    {
        if (jumpInput >= 1 && currentJumpDelay <= 0)
        {
            if (onWall) // WALL JUMP
            {
                joint2D.enabled = false;
                joint2D.connectedBody = null;
                wallJumpAvailable = false;
                SetSpeed(2);
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                rb2d.AddForce(new Vector2(transform.localScale.x * (wallJumpForce * 100) * -1, 0));
                currentJumpDelay = startingJumpDelay;
                if (AudioController.instance != null && !BlockAboveHead())
                {
                    AudioController.instance.PlayPlayerJump();
                }
            }
            else if (IsGrounded()) // GROUND JUMP
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                currentJumpDelay = startingJumpDelay;
                if (AudioController.instance != null && !BlockAboveHead())
                {
                    AudioController.instance.PlayPlayerJump();
                }
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) // INTERACTIONS WITH COLORED TILES
    {
        // IF A TILE IS INSIDE THE PLAYER, IGNORE THE COLLISION WITH IT (RE-ENABLES ON FIXED UPDATE())
        if (collision.gameObject.GetComponent<TileInteractable>() != null
            && Vector2.Distance(collision.transform.position, transform.position) <= minDistanceToIgnoreCol)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
            tileIgnoredForCollision = collision.gameObject;
        }
    }

    private void ReEnableCollision() // RE-ENABLES THE COLLISION WITH A TILE THAT WAS OVERLAPING THE PLAYER
    {
        if (tileIgnoredForCollision != null)
        {
            if (Vector2.Distance(tileIgnoredForCollision.transform.position, transform.position) > minDistanceToIgnoreCol)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), tileIgnoredForCollision.GetComponent<Collider2D>(), false);
                tileIgnoredForCollision = null;
            }
        }
    }

    private void Flip() // MAKES THE PLAYER FACE THE DIRECTION IT'S MOVING
    {
        if(rb2d.velocity.x < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            isFacingRight = false;
        }
        if(rb2d.velocity.x > 0 && !isFacingRight)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            isFacingRight = true;
        }

    }

    private void SmoothFall() // MAKES THE PLAYER FALL FASTER
    {
        if(rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (smoothFallMultiplier - 1) * Time.deltaTime;
        }
    }

    protected void SetSpeed(float amount) // SLOWS DOWN THE PLAYER BY X AMOUNT
    {
        currentMoveSpeed = amount; 
    }

    protected virtual void WallJump() // ALLOWS THE PLAYER TO WALLJUMP ON SOME PLATFORMS
    {

    }

    protected bool BlockAboveHead() // RETURNS TRUE IF THERE IS A BLOCK RIGHT ABOVE THE PLAYER HEAD
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y - groundCheckVerticalOffset);
        Vector2 size = new Vector2(groundRadius * 1.3f, groundRadius);
        return Physics2D.OverlapBox(position, size, 0, groundLayer);
    }

    private void OnDrawGizmosSelected() // DISPLAY INFO IN THE EDITOR
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + groundCheckVerticalOffset, 0), 
            new Vector3(groundRadius, groundRadius, 0));
    }
    #endregion

    #region Getters and Setters
    public float GetMovementAxis()
    {
        return movementAxis;
    }

    public void SetMovementAxis(float f)
    {
        movementAxis = f;
    }

    public void SetJumpAxis(float f)
    {
        jumpAxis = f;
    }
    #endregion
}
