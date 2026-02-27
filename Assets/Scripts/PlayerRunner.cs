using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jump & Physics")]
    public float jumpForce = 8f;
    public float gravity = -25f;
    public float jumpBufferTime = 0.2f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private int currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private bool canSwitchLane = true;

    private float horizontal;
    private float jumpBufferCounter;

    void Awake()
    {
        Time.timeScale = 1f;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    #region INPUT SYSTEM CALLBACKS

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        horizontal = input.x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    #endregion

    #region OPTIONAL MOBILE SUPPORT

    public void MoveInput(Vector2 input)
    {
        horizontal = input.x;
    }

    public void JumpInput(bool jumpPressed)
    {
        if (jumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    #endregion

    void Update()
    {
        // Stop if game not playing
        if (GameManager.Instance != null &&
            GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        // Jump buffer countdown
        if (jumpBufferCounter > 0)
            jumpBufferCounter -= Time.deltaTime;

        HandleLaneInput();

        // Calculate lane position
        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.MoveTowards(
            transform.position.x,
            targetX,
            laneSwitchSpeed * Time.deltaTime
        );

        // Ground check
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;

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

        // Movement vector
        Vector3 move = new Vector3(
            newX - transform.position.x,
            velocity.y,
            forwardSpeed
        );

        controller.Move(move * Time.deltaTime);

        // Animator updates
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
        {
            canSwitchLane = true;
        }
    }
}