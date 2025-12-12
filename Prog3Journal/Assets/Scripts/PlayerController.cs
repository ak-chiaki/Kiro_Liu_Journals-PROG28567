using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 40f;
    private Rigidbody2D rb;

    public Transform groundCheck;
    public float groundRadius = 0.1f;// Radius of the overlap circle
    public LayerMask groundLayer;
    
    private float inputX = 0f;// Stores horizontal input (-1, 0, 1)

    private FacingDirection currentFacingDirection = FacingDirection.right;

    [Header("Jump Settings")]
    public float apexHeight = 4.0f; 
    public float apexTime = 0.5f;   
    private float gravity;           
    private float initialJumpVelocity; // Calculated jump force based on apex settings
    private bool inputJump = false;

    public float terminalSpeed = 10f;

    public float coyoteTime = 0.5f; // Time window to jump after leaving a ledge
    private float coyoteTimeCounter;

    public float dashSpeed = 20f;
    [Header("Dash Settings")]
    public float dashDuration = 0.3f; 
    public float dashCooldown = 1.0f; 
    private float dashTimeLeft;       
    private float dashCooldownTimer;  
    private bool isDashing = false;
    private float dashDirection;

    [Header("Charge Jump Settings (Extra)")]
    public KeyCode chargeKey = KeyCode.C; // Dedicated key for charge jumping
    public float maxChargeTime = 1.5f;
    public float maxChargeJumpVelocity = 20f; // Jump force at full charge
    public float minChargeJumpVelocity = 5f;  // Jump force at minimum charge
    private float chargeTimeCounter = 0f;// Current charge timer
    private bool isCharging = false;


    public enum FacingDirection
    {
        left, right
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
       
        rb.gravityScale = 0f; // Disable default gravity to use our custom physics calculations

        gravity = -2 * apexHeight / (apexTime * apexTime);// Calculate gravity and jump velocity using kinematic formulas
        initialJumpVelocity = 2 * apexHeight / apexTime;


        PhysicsMaterial2D noFrictionMat = new PhysicsMaterial2D();// Create a frictionless material to prevent sticking to walls
        noFrictionMat.friction = 0f;      
        noFrictionMat.bounciness = 0f;   

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().sharedMaterial = noFrictionMat;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) inputX = -1;// Handle Horizontal Input (Left/Right Arrows)
        if (Input.GetKeyDown(KeyCode.RightArrow)) inputX = 1;
        if (Input.GetKeyUp(KeyCode.LeftArrow) && inputX == -1) inputX = 0;
        if (Input.GetKeyUp(KeyCode.RightArrow) && inputX == 1) inputX = 0;

        // Handle Normal Jump (Spacebar)
        // Only allow normal jump if not currently charging a super jump
        if (Input.GetKeyDown(KeyCode.Space) && !isCharging)
        {
            inputJump = true;
        }

        HandleChargeInput();

        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) && !isDashing && dashCooldownTimer <= 0)// Can only dash if not already dashing and cooldown has expired
        {
            StartDash();
        }

        Vector2 playerInput = new Vector2(inputX, inputJump ? 1f : 0f);
        MovementUpdate(playerInput);

        inputJump = false; // Reset
    }

    void HandleChargeInput()
    {
        if (IsGrounded() && !isDashing)// Can only start charging if on the ground and not dashing
        {
            if (Input.GetKeyDown(chargeKey))//Start Charging
            {
                isCharging = true;
                chargeTimeCounter = 0f;
            }

            if (Input.GetKey(chargeKey) && isCharging)//Accumulate Charge
            {
                chargeTimeCounter += Time.deltaTime;
                if (chargeTimeCounter > maxChargeTime) chargeTimeCounter = maxChargeTime;
            }

            if (Input.GetKeyUp(chargeKey) && isCharging)// Release to Jump
            {
                PerformChargeJump();
                isCharging = false;
                chargeTimeCounter = 0f;
            }
        }
        else// If player leaves ground or dashes, cancel charge
        {
            isCharging = false;
            chargeTimeCounter = 0f;
        }
    }

    void PerformChargeJump()// Calculates and applies the velocity for the Charge Jump
    {
        float ratio = chargeTimeCounter / maxChargeTime;
        float finalVelocity = Mathf.Lerp(minChargeJumpVelocity, maxChargeJumpVelocity, ratio);

        Vector2 v = rb.velocity;
        v.y = finalVelocity; 
        rb.velocity = v;

        coyoteTimeCounter = 0; 
    }

    private void MovementUpdate(Vector2 playerInput)// Main Physics Logic Loop
    {
        HandleDashTimers();

        Vector2 velocity = rb.velocity;

        if (IsGrounded())// If grounded, reset the timer. If in air, count down.
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < -terminalSpeed) velocity.y = -terminalSpeed;

        if (playerInput.y > 0 && coyoteTimeCounter > 0)// Jump is allowed if input is detected AND coyote time is valid
        {
            velocity.y = initialJumpVelocity;
            coyoteTimeCounter = 0;
        }

        if (isDashing)
        {
            velocity.x = dashDirection * dashSpeed;
        }
        else
        {
            if (isCharging)// Disable movement while charging
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
            }
            else// Standard Movement Logic
            {
                if (playerInput.x != 0)
                {
                    if (playerInput.x < 0) velocity.x -= acceleration * Time.deltaTime;
                    else if (playerInput.x > 0) velocity.x += acceleration * Time.deltaTime;
                }
                else
                {
                    if (velocity.x > 0)// Deceleration
                    {
                        velocity.x -= deceleration * Time.deltaTime;
                        if (velocity.x < 0) velocity.x = 0;
                    }
                    else if (velocity.x < 0)
                    {
                        velocity.x += deceleration * Time.deltaTime;
                        if (velocity.x > 0) velocity.x = 0;
                    }
                }

                if (velocity.x > maxSpeed) velocity.x = maxSpeed;// Clamp Horizontal Speed
                if (velocity.x < -maxSpeed) velocity.x = -maxSpeed;
            }
        }

        rb.velocity = velocity;

        if (playerInput.x > 0 && !isCharging)// Update Facing Direction based on input
            currentFacingDirection = FacingDirection.right;
        else if (playerInput.x < 0 && !isCharging)
            currentFacingDirection = FacingDirection.left;
    }
    void StartDash()// Logic for the "Dash" Proposal
    {
        isCharging = false;
        isDashing = true;
        dashTimeLeft = dashDuration;

        if (inputX != 0)// Determine dash direction based on input or current facing direction
        {
            dashDirection = inputX;
        }
        else
        {
            dashDirection = (currentFacingDirection == FacingDirection.right) ? 1f : -1f;
        }
    }


    void HandleDashTimers()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime; 

            if (dashTimeLeft <= 0)
            {
                isDashing = false; 
                dashCooldownTimer = dashCooldown; 
            }
        }
    }

    public bool IsWalking()
    {
        float vx = rb.velocity.x;
        if (vx != 0)
            return true;
        else
            return false;
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);// Physics overlap check for ground layer
    }
    public FacingDirection GetFacingDirection()
    {
        return currentFacingDirection;
    }
    public void LaunchPlayer(float launchForce)// This method is called externally by the Spring object
    {
        Vector2 v = rb.velocity;
        v.y = 0;
        v.y = launchForce;// Apply launch force
        rb.velocity = v;

        isDashing = false;       
        coyoteTimeCounter = 0;  // Cannot jump again immediately
        inputJump = false;      
    }

}


