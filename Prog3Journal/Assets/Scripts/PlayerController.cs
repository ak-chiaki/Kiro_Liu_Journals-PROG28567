using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 40f;
    private Rigidbody2D rb;

    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;
    
    private float inputX = 0f;

    private FacingDirection currentFacingDirection = FacingDirection.right;

    [Header("Jump Settings")]
    public float apexHeight = 4.0f; 
    public float apexTime = 0.5f;   
    private float gravity;           
    private float initialJumpVelocity; 
    private bool inputJump = false;

    public float terminalSpeed = 10f;

    public float coyoteTime = 0.5f; 
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
    public KeyCode chargeKey = KeyCode.C; 
    public float maxChargeTime = 1.5f;
    public float maxChargeJumpVelocity = 20f; 
    public float minChargeJumpVelocity = 5f;  
    private float chargeTimeCounter = 0f;
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
        rb.gravityScale = 0f;

        gravity = -2 * apexHeight / (apexTime * apexTime);
        initialJumpVelocity = 2 * apexHeight / apexTime;


        PhysicsMaterial2D noFrictionMat = new PhysicsMaterial2D();
        noFrictionMat.friction = 0f;      
        noFrictionMat.bounciness = 0f;   

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().sharedMaterial = noFrictionMat;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) inputX = -1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) inputX = 1;
        if (Input.GetKeyUp(KeyCode.LeftArrow) && inputX == -1) inputX = 0;
        if (Input.GetKeyUp(KeyCode.RightArrow) && inputX == 1) inputX = 0;

        if (Input.GetKeyDown(KeyCode.Space) && !isCharging)
        {
            inputJump = true;
        }

        HandleChargeInput();

        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) && !isDashing && dashCooldownTimer <= 0)
        {
            StartDash();
        }

        Vector2 playerInput = new Vector2(inputX, inputJump ? 1f : 0f);
        MovementUpdate(playerInput);

        inputJump = false; 
    }

    void HandleChargeInput()
    {
        if (IsGrounded() && !isDashing)
        {
            if (Input.GetKeyDown(chargeKey))
            {
                isCharging = true;
                chargeTimeCounter = 0f;
            }

            if (Input.GetKey(chargeKey) && isCharging)
            {
                chargeTimeCounter += Time.deltaTime;
                if (chargeTimeCounter > maxChargeTime) chargeTimeCounter = maxChargeTime;
            }

            if (Input.GetKeyUp(chargeKey) && isCharging)
            {
                PerformChargeJump();
                isCharging = false;
                chargeTimeCounter = 0f;
            }
        }
        else
        {
            isCharging = false;
            chargeTimeCounter = 0f;
        }
    }

    void PerformChargeJump()
    {
        float ratio = chargeTimeCounter / maxChargeTime;
        float finalVelocity = Mathf.Lerp(minChargeJumpVelocity, maxChargeJumpVelocity, ratio);

        Vector2 v = rb.velocity;
        v.y = finalVelocity; 
        rb.velocity = v;

        coyoteTimeCounter = 0; 
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        HandleDashTimers();

        Vector2 velocity = rb.velocity;

        if (IsGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < -terminalSpeed) velocity.y = -terminalSpeed;

        if (playerInput.y > 0 && coyoteTimeCounter > 0)
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
            if (isCharging)
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
            }
            else
            {
                if (playerInput.x != 0)
                {
                    if (playerInput.x < 0) velocity.x -= acceleration * Time.deltaTime;
                    else if (playerInput.x > 0) velocity.x += acceleration * Time.deltaTime;
                }
                else
                {
                    if (velocity.x > 0)
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

                if (velocity.x > maxSpeed) velocity.x = maxSpeed;
                if (velocity.x < -maxSpeed) velocity.x = -maxSpeed;
            }
        }

        rb.velocity = velocity;

        if (playerInput.x > 0 && !isCharging)
            currentFacingDirection = FacingDirection.right;
        else if (playerInput.x < 0 && !isCharging)
            currentFacingDirection = FacingDirection.left;
    }
    void StartDash()
    {
        isCharging = false;
        isDashing = true;
        dashTimeLeft = dashDuration;

        if (inputX != 0)
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
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
    public FacingDirection GetFacingDirection()
    {
        return currentFacingDirection;
    }
    public void LaunchPlayer(float launchForce)
    {
        Vector2 v = rb.velocity;
        v.y = 0;
        v.y = launchForce;
        rb.velocity = v;

        isDashing = false;       
        coyoteTimeCounter = 0;  
        inputJump = false;      
    }

}


