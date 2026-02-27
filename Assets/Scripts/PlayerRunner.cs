using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float speedIncreaseRate = 0.1f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jump & Physics")]
    public float jumpForce = 12f;
    public float gravity = -30f;
    public float jumpBufferTime = 0.2f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private int currentLane = 1;
    private bool inputReset = true;

    private float horizontal;
    private float jumpBufferCounter;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    #region Input
    public void MoveInput(Vector2 newMoveDirection)
    {
        horizontal = newMoveDirection.x;
    }

    // 2. Ensure this is PUBLIC and accepts a BOOL
    public void JumpInput(bool pressed)
    {
        if (pressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }
    #endregion

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        // 1. Timers & Speed
        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;
        forwardSpeed += speedIncreaseRate * Time.deltaTime;

        // 2. Lane Logic
        HandleLanes();

        // 3. Physics & Movement
        ApplyMovement();

        // 4. Animations
        UpdateAnimations();
    }

    private void HandleLanes()
    {
        if (inputReset)
        {
            if (horizontal > 0.5f && currentLane < 2)
            {
                currentLane++;
                inputReset = false;
            }
            else if (horizontal < -0.5f && currentLane > 0)
            {
                currentLane--;
                inputReset = false;
            }
        }

        if (Mathf.Abs(horizontal) < 0.15f) inputReset = true;
    }

    private void ApplyMovement()
    {
        
        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);
        float xDelta = newX - transform.position.x;

        
        if (controller.isGrounded)
        {
            if (velocity.y < 0) velocity.y = -1f;

            if (jumpBufferCounter > 0)
            {
                velocity.y = jumpForce;
                jumpBufferCounter = 0;
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        
        Vector3 move = new Vector3(xDelta, velocity.y * Time.deltaTime, forwardSpeed * Time.deltaTime);
        controller.Move(move);
    }

    private void UpdateAnimations()
    {
        animator.SetFloat("Speed", forwardSpeed);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }
}