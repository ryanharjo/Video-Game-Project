using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float maxSpeed = 30f;
    public float speedIncreaseRate = 0.1f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jump & Physics")]
    public float jumpForce = 8f;
    public float gravity = -25f;
    public float jumpBufferTime = 0.2f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity; // Vertical and forward velocity
    private int currentLane = 1; // 0: Left, 1: Middle, 2: Right
    private bool canSwitchLane = true;

    // Input States & Buffers
    private float horizontal;
    private float jumpBufferCounter;

    void Awake()
    {
        Time.timeScale = 1f;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    #region Input Callbacks
    public void Move(InputAction.CallbackContext context) => MoveInput(context.ReadValue<Vector2>());
    public void Jump(InputAction.CallbackContext context) => JumpInput(context.ReadValueAsButton());

    public void MoveInput(Vector2 newMoveDirection) => horizontal = newMoveDirection.x;

    public void JumpInput(bool pressed)
    {
        if (virtualJumpState)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }
    #endregion

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        // 1. Manage Timers
        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;

        // 3. Lane Logic
        HandleLaneInput();

        // Calculate Target X based on lane
        float targetX = (currentLane - 1) * laneDistance;
        Vector3 nextPosition = transform.position;

        // Smoothly move the X coordinate toward the target lane
        nextPosition.x = Mathf.MoveTowards(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);

        // 3. Vertical Movement (Jump & Gravity)
        if (controller.isGrounded)
        {
            // Small downward force to keep isGrounded stable
            if (direction.y < 0) direction.y = -2f;

            // Trigger Jump if buffered
            if (jumpBufferCounter > 0)
            {
                velocity.y = jumpForce;
                jumpBufferCounter = 0;
                animator.SetTrigger("Jump"); // Use Trigger for more reliable animation
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
        }

        // 4. Apply Movement
        direction.x = xVelocity;
        direction.z = forwardSpeed;

        // Combine all movements: (X: Lane change, Y: Jump/Gravity, Z: Constant Forward)
        Vector3 moveDelta = new Vector3(xMovement, velocity.y * Time.deltaTime, forwardSpeed * Time.deltaTime);

        // 5. Update Animations
        animator.SetFloat("Speed", 1f);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void HandleLaneInput()
    {
        // Check for "Press"
        if (canSwitchLane)
        {
            if (horizontal > 0.5f && currentLane < 2)
            {
                currentLane++;
                canSwitchLane = false;
            }
            else if (horizontal < -0.5f && currentLane > 0)
            {
                currentLane--;
                canSwitchLane = false;
            }
        }

        // Check for "Release" - this allows the player to switch again
        if (Mathf.Abs(horizontal) < 0.1f)
        {
            canSwitchLane = true;
        }
    }
}