using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jump Settings")]
    public float jumpForce = 6f;
    public float gravity = -20f;

    private CharacterController controller;
    private Animator animator;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 mobileMoveInput;
    private bool mobileJumpInput;

    private Vector3 velocity;
    private int currentLane = 1;
    private bool canSwitchLane = true;

    private bool wasGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        Debug.Log("PlayerRunner initialized.");
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        UpdateAnimator();
        DebugGroundState();
    }

    void HandleMovement()
    {
        float targetX = (currentLane - 1) * laneDistance;

        Vector3 currentPosition = transform.position;
        float newX = Mathf.MoveTowards(
            currentPosition.x,
            targetX,
            laneSwitchSpeed * Time.deltaTime
        );

        float xDelta = newX - currentPosition.x;

        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;

            bool jumpPressed = jumpAction.triggered || mobileJumpInput;
            if (jumpPressed)
            {
                Debug.Log("JUMP TRIGGERED");
                velocity.y = jumpForce;

                if (animator != null)
                    animator.SetTrigger("Jump");

                mobileJumpInput = false; // reset mobile tap
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        Vector3 moveVector = new Vector3(
            xDelta,
            velocity.y,
            forwardSpeed * Time.deltaTime
        );

        controller.Move(moveVector);
    }

    void HandleInput()
    {
        // Read Input System value (keyboard/controller)
        float inputSystemX = moveAction.ReadValue<Vector2>().x;

        // Combine with mobile joystick input
        float horizontalInput = inputSystemX;

        if (Mathf.Abs(mobileMoveInput.x) > Mathf.Abs(horizontalInput))
            horizontalInput = mobileMoveInput.x;

        if (canSwitchLane)
        {
            if (horizontalInput > 0.5f && currentLane < 2)
            {
                currentLane++;
                canSwitchLane = false;
                Debug.Log("Switched to lane: " + currentLane);
            }
            else if (horizontalInput < -0.5f && currentLane > 0)
            {
                currentLane--;
                canSwitchLane = false;
                Debug.Log("Switched to lane: " + currentLane);
            }
        }

        if (Mathf.Abs(horizontalInput) < 0.1f)
            canSwitchLane = true;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetBool("IsRunning", true);
        animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetFloat("VerticalVelocity", velocity.y);
    }

    void DebugGroundState()
    {
        Debug.Log("Grounded: " + controller.isGrounded);
        if (controller.isGrounded && !wasGrounded)
        {
            Debug.Log("LANDED");
        }
        else if (!controller.isGrounded && wasGrounded)
        {
            Debug.Log("LEFT GROUND");
        }

        wasGrounded = controller.isGrounded;
    }
  
    public void MoveInput(Vector2 value)
    {
        mobileMoveInput = value;
    }

    public void JumpInput(bool value)
    {
        mobileJumpInput = value;
    }
}