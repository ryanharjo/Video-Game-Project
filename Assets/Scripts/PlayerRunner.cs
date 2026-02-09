using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float gravity = -25f;

    [Header("Slide")]
    public float slideTime = 0.8f;
    public float slideHeightMultiplier = 0.5f;

    private CharacterController controller;
    private Animator animator;
    private PlayerInput playerInput;

    private Vector3 direction;
    private int currentLane = 1;
    private bool canSwitchLane = true;
    private bool isSliding = false;

    // INPUT STATES
    private float horizontal;
    private float vertical;
    private bool hasJumped;
    private bool hasSlid;
    private bool hasFlipped;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    // INPUT CALLBACKS (Input System)
    public void Move(InputAction.CallbackContext context)
    {
        MoveInput(context.ReadValue<Vector2>());
    }

    public void Jump(InputAction.CallbackContext context)
    {
        JumpInput(context.ReadValueAsButton());
    }

    public void Slide(InputAction.CallbackContext context)
    {
        SlideInput(context.ReadValueAsButton());
    }

    public void Flip(InputAction.CallbackContext context)
    {
        FlipInput(context.ReadValueAsButton());
    }

    // WRAPPERS (for mobile & testing)
    public void MoveInput(Vector2 newMoveDirection)
    {
        horizontal = newMoveDirection.x;
        vertical = newMoveDirection.y;
    }

    public void JumpInput(bool newValue)
    {
        hasJumped = newValue;
    }

    public void SlideInput(bool newValue)
    {
        hasSlid = newValue;
    }

    public void FlipInput(bool newValue)
    {
        hasFlipped = newValue;
    }

    void Update()
    {
        HandleLaneInput();

        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.Lerp(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);
        float xDelta = newX - transform.position.x;

        if (controller.isGrounded)
        {
            if (direction.y < 0) direction.y = -2f;

            if (hasJumped && !isSliding)
            {
                direction.y = jumpForce;
                animator.SetBool("Jump", true);
                hasJumped = false;
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

        // Slide
        if (hasSlid && controller.isGrounded && !isSliding)
        {
            StartCoroutine(SlideRoutine());
            hasSlid = false;
        }

        // Flip
        if (hasFlipped && controller.isGrounded && !isSliding)
        {
            animator.SetTrigger("Flip");
            hasFlipped = false;
        }

        direction.x = xDelta;
        direction.z = forwardSpeed;

        controller.Move(direction * Time.deltaTime);

        animator.SetFloat("Speed", 1f);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void HandleLaneInput()
    {
        float h = horizontal;

        if (canSwitchLane)
        {
            if (h > 0.5f && currentLane < 2)
            {
                currentLane++;
                canSwitchLane = false;
            }
            else if (h < -0.5f && currentLane > 0)
            {
                currentLane--;
                canSwitchLane = false;
            }
        }

        if (Mathf.Abs(h) < 0.1f)
            canSwitchLane = true;
    }

    IEnumerator SlideRoutine()
    {
        isSliding = true;
        animator.SetBool("Slide", true);

        float originalHeight = controller.height;
        Vector3 originalCenter = controller.center;

        controller.height = originalHeight * slideHeightMultiplier;
        controller.center = originalCenter * slideHeightMultiplier;

        yield return new WaitForSeconds(slideTime);

        controller.height = originalHeight;
        controller.center = originalCenter;

        animator.SetBool("Slide", false);
        isSliding = false;
    }
}