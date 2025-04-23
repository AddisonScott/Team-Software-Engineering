using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float fallMultiplier = 2.5f;
    public float jumpMultiplier = 2.5f;
    public float jumpCounter;
    public float jumpTime;
    public Transform groundCheck;
    

    private Rigidbody2D rb;
    

    bool isJumping;
    bool isGrounded;
    Vector2 vecGravity;

    Vector2 moveDirection = Vector2.zero;

    
    private void OnEnable()
    {
        vecGravity = new Vector2(0f, -Physics2D.gravity.y); // Increases the gravity when falling
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
    }
    void Update()
    {
        // Check if the player is touching the ground
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.5f, 0.1f), CapsuleDirection2D.Horizontal, 0f, LayerMask.GetMask("Ground"));

        // Increase gravity when falling
        if (rb.linearVelocity.y < 0 )
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y - (fallMultiplier * Time.deltaTime)); // Increases the gravity when falling
        }
        // Decrease gravity when jumping
        if ( rb.linearVelocity.y>0 && isJumping)
        {
            jumpCounter +=Time.deltaTime;
            if (jumpCounter > jumpTime)
            {
                isJumping = false;
            }

            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5)
            {
                currentJumpM = jumpMultiplier * (1 - t); // Decreases the jump height over time  
                
            }
            rb.linearVelocity += vecGravity * currentJumpM * Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        // Smoother Movement
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 targetVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y);
        float acceleration = 10f; 
        Vector2 smoothedVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime); // Curves the velocity change
        rb.linearVelocity = smoothedVelocity;
    }

    /// Input System
    public void Jump(InputAction.CallbackContext context)
    {
        // Space down
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Jumping
            isJumping = true;
            jumpCounter = 0f;
        }
        // Space up
        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            isJumping = false;
            jumpCounter = 0f;

            if(rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.6f); // Cut jump short
            }
        }
    }

    /// Input System
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDirection = Vector2.zero;
        }
    }
}

