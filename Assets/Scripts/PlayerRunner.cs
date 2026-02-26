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
    private float horizontal;
    private float jumpBufferCounter;

    void Awake()
    {
        Time.timeScale = 1f;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    #region Input Callbacks
    public void JumpInput(bool virtualJumpState)
    {
        if (virtualJumpState)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    // Add this for the Mobile UI Joysticks/D-pad to call
    public void MoveInput(Vector2 virtualMoveDirection)
    {
        horizontal = virtualMoveDirection.x;
    }

    // Keep your existing Input System callbacks for Keyboard/Controller
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started) jumpBufferCounter = jumpBufferTime;
    }

    #endregion

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        // 1. Gradually increase speed over time
        if (forwardSpeed < maxSpeed)
            forwardSpeed += speedIncreaseRate * Time.deltaTime;

        // 2. Timers
        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;

        // 3. Lane Logic
        HandleLaneInput();

        // Calculate Target X based on lane
        float targetX = (currentLane - 1) * laneDistance;
        Vector3 nextPosition = transform.position;

        // Smoothly move the X coordinate toward the target lane
        nextPosition.x = Mathf.MoveTowards(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);

        // 4. Vertical Logic (Gravity & Jump)
        if (controller.isGrounded)
        {
            velocity.y = -1f; // Small downward force to stay grounded

            if (jumpBufferCounter > 0)
            {
                velocity.y = jumpForce;
                jumpBufferCounter = 0;
                animator.SetTrigger("Jump"); // Use Trigger for more reliable animation
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // 5. Apply Movement
        // Calculate the difference needed to reach the next X position
        float xMovement = nextPosition.x - transform.position.x;

        // Combine all movements: (X: Lane change, Y: Jump/Gravity, Z: Constant Forward)
        Vector3 moveDelta = new Vector3(xMovement, velocity.y * Time.deltaTime, forwardSpeed * Time.deltaTime);

        controller.Move(moveDelta);

        // 6. Animator Updates
        animator.SetFloat("Speed", forwardSpeed / 10f); // Scales animation speed with player speed
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

    // Detection for hitting obstacles
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Assuming obstacles have a "Obstacle" tag
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            // GameManager.Instance.GameOver(); 
        }
    }
}