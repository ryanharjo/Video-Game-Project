using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]

    // Constant forward movement speed of the player
    public float forwardSpeed = 10f;

    // Distance between each running lane
    public float laneDistance = 3f;

    // Speed at which the player switches lanes
    public float laneSwitchSpeed = 10f;

    [Header("Jump")]

    // Initial upward force applied when jumping
    public float jumpForce = 6f;

    // Downward force applied every frame while airborne
    public float gravity = -20f;

    // Optional property for modifying player speed (useful for powerups)
    public float PlayerSpeed => forwardSpeed;

    // Reference to CharacterController component
    private CharacterController controller;

    // Reference to Animator component for controlling animations
    private Animator anim;

    // Horizontal input value from Input System
    private float horizontal;

    // Vertical input value from Input System (currently unused)
    private float vertical;

    // Controls upward and downward movement
    private float verticalVelocity;

    // Current lane index (-1 = left, 0 = middle, 1 = right)
    private int currentLane = 0;

    // Maximum amount of time the player can remain in a jump
    private float jumpTime = 1.0f;

    // Timer used to limit jump duration
    private float jumpTimer;

    // Tracks whether jump input is currently pressed
    private bool hasJumped = false;

    // Prevents multiple jumps while airborne
    private bool canJump = true;

    // Indicates whether the player is touching the ground
    private bool groundedPlayer;

    /// <summary>
    /// Called once when the game starts.
    /// Initializes component references and jump timer.
    /// </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        // Reset jump timer at start
        jumpTimer = 0.0f;
    }

    /// <summary>
    /// Called every frame.
    /// Handles forward movement, lane switching, gravity, jumping,
    /// and updates animation parameters.
    /// </summary>
    void Update()
    {
        // Move player forward continuously
        Vector3 move = Vector3.forward * forwardSpeed;

        // Check if player is grounded
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            // Prevents unwanted downward force when grounded
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            anim.SetBool("IsGrounded", true);

            // Jump logic using Input System and jump cooldown
            if (hasJumped && canJump)
            {
                verticalVelocity = jumpForce;

                // Start jump cooldown
                jumpTimer = jumpTime;
                canJump = false;
                groundedPlayer = false;

                anim.SetTrigger("Jump");
            }
        }
        else
        {
            anim.SetBool("IsGrounded", false);
        }

        // Handles jump cooldown timing
        if (!canJump)
        {
            jumpTimer -= Time.deltaTime;

            if (jumpTimer <= 0)
            {
                jumpTimer = 0;
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }

        // Apply gravity over time
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        // Change lanes based on horizontal input
        if (horizontal < -0.5f)
            ChangeLane(-1);
        else if (horizontal > 0.5f)
            ChangeLane(1);

        // Smooth movement toward target lane
        float targetX = currentLane * laneDistance;
        float deltaX = targetX - transform.position.x;
        move.x = deltaX * laneSwitchSpeed;

        // Apply movement to CharacterController
        controller.Move(move * Time.deltaTime);

        // Update animation blend based on speed
        anim.SetFloat("Speed", forwardSpeed);
    }

    /// <summary>
    /// Receives movement input from the Input System.
    /// Stores horizontal and vertical input values.
    /// </summary>
    /// <param name="context">Input callback context</param>
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        horizontal = input.x;
        vertical = input.y;
    }

    /// <summary>
    /// Receives jump input from the Input System.
    /// Sets jump state based on button press.
    /// </summary>
    /// <param name="context">Input callback context</param>
    public void Jump(InputAction.CallbackContext context)
    {
        hasJumped = context.ReadValueAsButton();
    }

    /// <summary>
    /// Changes the player's current lane while keeping it within bounds.
    /// </summary>
    /// <param name="direction">Direction to move (-1 left, 1 right)</param>
    void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }
}
