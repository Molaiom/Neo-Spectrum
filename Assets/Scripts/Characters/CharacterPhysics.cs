using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysics : MonoBehaviour
{
    #region Attributes
    [Header("Attributes")]
    public float startingMoveSpeed;
    public float jumpForce;
    public float wallJumpForce;

    [Header("Unity Components")]
    public float smoothFallMultiplier;
    public float groundRadius;
    public LayerMask groundLayer;
    public float groundCheckVerticalOffset;

    protected bool isFacingRight = true;
    protected bool isGrounded;
    protected bool wallJumpAvailable;
    protected bool onWall;
    protected float currentMoveSpeed;
    protected Rigidbody2D rb2d;
    protected FixedJoint2D joint2D;

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
        CheckGround();
        HorizontalMovement(movementAxis);
        Jump(jumpAxis);
        SmoothFall();
        Flip();
        WallJump();
    }
    #endregion

    #region Methods
    private void CheckGround()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y + groundCheckVerticalOffset);
        Vector2 size = new Vector2(groundRadius, groundRadius);
        isGrounded = Physics2D.OverlapBox(position, size, 0, groundLayer);
    }

    private void HorizontalMovement(float direction) // MAKES THE PLAYER MOVE
    {
        currentMoveSpeed += 5 * Time.deltaTime;
        currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, 0.5f, startingMoveSpeed);

        rb2d.velocity = new Vector2(direction * currentMoveSpeed, rb2d.velocity.y);


    }

    private void Jump(float jumpInput) // MAKES THE PLAYER JUMP 
    {
        if (jumpInput >= 1)
        {
            if (onWall) // WALL JUMP
            {
                joint2D.enabled = false;
                joint2D.connectedBody = null;
                wallJumpAvailable = false;
                SetSpeed(2);
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                rb2d.AddForce(new Vector2(transform.localScale.x * (wallJumpForce * 100) * -1, 0));
                if (AudioController.instance != null && !BlockAboveHead())
                {
                    AudioController.instance.PlayPlayerJump();
                }
            }
            else if (isGrounded) // GROUND JUMP
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                if (AudioController.instance != null && !BlockAboveHead())
                {
                    AudioController.instance.PlayPlayerJump();
                }
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

    public bool GetIsGrounded()
    {
        return isGrounded;
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
