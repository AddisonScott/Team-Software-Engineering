using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float fallMultiplier = 2.5f;
    public float jumpMultiplier = 2.5f;
    public float jumpCounter;
    public float jumpTime;
    public float flipDuration;
    public Transform groundCheck;
    public ParticleSystem[] dustSystem;
    private Rigidbody2D rb;

    bool wasMoving = false; // To check if the player is moving or not
    bool facingRight;
    bool isMoving;
    bool isTurning;
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
        // Jumping
        // Check if the player is touching the ground
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.5f, 0.1f), CapsuleDirection2D.Horizontal, 0f, LayerMask.GetMask("Ground"));

        // Increase gravity when falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y - (fallMultiplier * Time.deltaTime)); // Increases the gravity when falling
        }
        // Decrease gravity when jumping
        if (rb.linearVelocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
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

        float threshold = 0.1f;
        bool movingNow = isGrounded && Mathf.Abs(rb.linearVelocity.x) > threshold;

        int rotationSpeed = 20;

        // Dust effect
        if (movingNow && !wasMoving)
        {

            foreach (var ps in dustSystem)
            {
                ps.Play();  
            }
            print("Dust effect on");
        }
        else if (!movingNow && wasMoving)
        {
            foreach (var ps in dustSystem)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting); // Stops emitting new particles but lets old ones finish
            }
            print("Dust effect off");
        }
        wasMoving = movingNow;

        // Flip the player sprite
        if (isTurning)
        {
            StartCoroutine(FlipPlayer());
        }



    }

    private IEnumerator FlipPlayer()
    {;
        float elapsed = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0f, facingRight ? 180f : 0f, 0f); 

        while (elapsed < flipDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / flipDuration); // Smoothly interpolate between the start and end rotation
            elapsed += Time.deltaTime; 
            yield return null; // Each frame
        }

        transform.rotation = endRotation;
        isTurning = false;
    }

    private void FixedUpdate()
    {
        // Smoother Movement
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 targetVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y);
        float acceleration = 10f; 
        Vector2 smoothedVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime); // Smoothly transitions between the two values
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
            isTurning = true;
            moveDirection = context.ReadValue<Vector2>();
            if (moveDirection.x > 0)
            {
                facingRight = false;
            }
            else
            {
                facingRight = true;
            }
        }
        else if (context.canceled)
        {
            isTurning = false;
            moveDirection = Vector2.zero;
        }
    }


}

