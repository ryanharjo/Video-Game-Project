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

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction slideAction;
    private InputAction flipAction;

    private Vector3 direction;
    private int currentLane = 1;
    private bool canSwitchLane = true;
    private bool isSliding = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        slideAction = playerInput.actions["Slide"];
        flipAction = playerInput.actions["Flip"];
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        slideAction.Enable();
        flipAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        slideAction.Disable();
        flipAction.Disable();
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

            if (jumpAction.triggered && !isSliding)
            {
                direction.y = jumpForce;
                animator.SetBool("Jump", true);
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
        if (slideAction.triggered && controller.isGrounded && !isSliding)
            StartCoroutine(SlideRoutine());

        // Flip
        if (flipAction.triggered && controller.isGrounded && !isSliding)
            animator.SetTrigger("Flip");

        direction.x = xDelta;
        direction.z = forwardSpeed;

        controller.Move(direction * Time.deltaTime);

        animator.SetFloat("Speed", 1f);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void HandleLaneInput()
    {
        float horizontal = moveAction.ReadValue<Vector2>().x;

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
