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
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            inputX = -1;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            inputX = 1;

        if (Input.GetKeyUp(KeyCode.LeftArrow) && inputX == -1)
            inputX = 0;

        if (Input.GetKeyUp(KeyCode.RightArrow) && inputX == 1)
            inputX = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputJump = true;
        }

        Vector2 playerInput = new Vector2(inputX, inputJump ? 1f : 0f);
        MovementUpdate(playerInput);

        inputJump = false;

    }

    private void MovementUpdate(Vector2 playerInput)
    {
        Vector2 velocity = rb.velocity;

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        velocity.y += gravity * Time.deltaTime;

        if (velocity.y < -terminalSpeed)
        {
            velocity.y = -terminalSpeed; 
        }

        if (playerInput.y > 0 && coyoteTimeCounter > 0)
        {
            velocity.y = initialJumpVelocity;
            coyoteTimeCounter = 0;
        }

        if (playerInput.x != 0)
        {
            if (playerInput.x < 0)
            {
                velocity.x -= acceleration * Time.deltaTime;
            }
            else if (playerInput.x > 0)
            {
                velocity.x += acceleration * Time.deltaTime;
            }
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

        if (velocity.x > maxSpeed)
            velocity.x = maxSpeed;

        if (velocity.x < -maxSpeed)
            velocity.x = -maxSpeed;

        rb.velocity = velocity;

        if (playerInput.x > 0)
        {
            currentFacingDirection = FacingDirection.right;
        }
        else if (playerInput.x < 0)
        {
            currentFacingDirection = FacingDirection.left;
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
}
