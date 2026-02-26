using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jump & Physics")]
    public float jumpForce = 7.5f;
    public float gravity = -25f;
    public float jumpBufferTime = 0.2f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 direction;
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
        if (pressed) jumpBufferCounter = jumpBufferTime;
    }
    #endregion

    void Update()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.Playing) return;

        // 1. Manage Timers
        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;

        // 2. Horizontal Movement (Lanes)
        HandleLaneInput();
        float targetX = (currentLane - 1) * laneDistance;

        // Calculate the speed needed to reach the target X this frame
        float newX = Mathf.MoveTowards(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);
        float xVelocity = (newX - transform.position.x) / Time.deltaTime;

        // 3. Vertical Movement (Jump & Gravity)
        if (controller.isGrounded)
        {
            // Small downward force to keep isGrounded stable
            if (direction.y < 0) direction.y = -2f;

            // Trigger Jump if buffered
            if (jumpBufferCounter > 0)
            {
                direction.y = jumpForce;
                animator.SetBool("Jump", true);
                jumpBufferCounter = 0; // Clear buffer
            }
            else
            {
                animator.SetBool("Jump", false);
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
        }

        // 4. Apply Movement
        direction.x = xVelocity;
        direction.z = forwardSpeed;

        controller.Move(direction * Time.deltaTime);

        // 5. Update Animations
        animator.SetFloat("Speed", 1f);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void HandleLaneInput()
    {
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

        if (Mathf.Abs(horizontal) < 0.1f)
            canSwitchLane = true;
    }
}