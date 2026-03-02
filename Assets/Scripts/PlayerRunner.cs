using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jump Settings")]
    public float jumpForce = 5.5f;   // Lower jump
    public float gravity = -32f;     // Stronger gravity
    public float fallMultiplier = 1.5f; // Faster falling

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

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
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
    }

    void HandleMovement()
    {
        // Lane movement
        float targetX = (currentLane - 1) * laneDistance;

        Vector3 currentPosition = transform.position;
        float newX = Mathf.MoveTowards(
            currentPosition.x,
            targetX,
            laneSwitchSpeed * Time.deltaTime
        );

        float xDelta = newX - currentPosition.x;

        // ALWAYS apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Faster falling (better runner feel)
        if (velocity.y < 0)
        {
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Jump
        bool jumpPressed = jumpAction.triggered || mobileJumpInput;

        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f; // Stick to ground

            if (jumpPressed)
            {
                velocity.y = jumpForce;

                if (animator != null)
                    animator.SetTrigger("Jump");

                mobileJumpInput = false;
            }
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
        float inputSystemX = moveAction.ReadValue<Vector2>().x;

        float horizontalInput = inputSystemX;

        if (Mathf.Abs(mobileMoveInput.x) > Mathf.Abs(horizontalInput))
            horizontalInput = mobileMoveInput.x;

        if (canSwitchLane)
        {
            if (horizontalInput > 0.5f && currentLane < 2)
            {
                currentLane++;
                canSwitchLane = false;
            }
            else if (horizontalInput < -0.5f && currentLane > 0)
            {
                currentLane--;
                canSwitchLane = false;
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

    public void MoveInput(Vector2 value)
    {
        mobileMoveInput = value;
    }

    public void JumpInput(bool value)
    {
        mobileJumpInput = value;
    }
}