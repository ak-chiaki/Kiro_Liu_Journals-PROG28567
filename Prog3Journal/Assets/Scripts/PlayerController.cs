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

        Vector2 playerInput = new Vector2(inputX, 0f);
        MovementUpdate(playerInput);
        Debug.Log(IsGrounded());
    }

    private void MovementUpdate(Vector2 playerInput)
    {

        Vector2 velocity = rb.velocity;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
         
            if (playerInput.x < 0)
                velocity.x -= acceleration * Time.deltaTime;

            
            else if (playerInput.x > 0)
                velocity.x += acceleration * Time.deltaTime;
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
