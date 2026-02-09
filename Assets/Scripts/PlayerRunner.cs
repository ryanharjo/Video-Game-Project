using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerRunner : MonoBehaviour
{
    
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float gravity = -25f;
    private bool canJump = true;
    
    public float jumpForce = 6f;
    public float gravity = -20f;

    [Header("Slide")]
    public float slideTime = 0.8f;
    public float slideHeightMultiplier = 0.5f;

    // Physics
    private Vector3 direction; 
    private int currentLane = 1; 
    private Animator animator;
    private PlayerInput playerInput;
    public float jumpForce = 6f;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction slideAction;
    private InputAction flipAction;
    private Animator anim;
    
    private bool canSwitchLane = true;

    void Awake()
    private Vector3 direction;
    private int currentLane = 1;
    private bool canSwitchLane = true;
    private bool isSliding = false;

    void Awake()
    private float verticalVelocity;
    private bool groundedPlayer;
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
    void Start()
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        slideAction = playerInput.actions["Slide"];
        flipAction = playerInput.actions["Flip"];
        anim = GetComponentInChildren<Animator>();

    void OnEnable() { moveAction.Enable(); jumpAction.Enable(); }
    void OnDisable() { moveAction.Disable(); jumpAction.Disable(); }

    void Update()
    void OnEnable()
    }
        HandleInput();
        moveAction.Enable();
        jumpAction.Enable();
        slideAction.Enable();
        flipAction.Enable();
    }
    {
        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.Lerp(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);
        float xDelta = newX - transform.position.x;

        if (controller.isGrounded)
        {
            if (direction.y < 0) direction.y = -2f;
            if (jumpAction.triggered) direction.y = jumpForce;
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
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

        direction.x = xDelta;
        direction.z = forwardSpeed;
        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.Lerp(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);
        float xDelta = newX - transform.position.x;
        groundedPlayer = controller.isGrounded;
        controller.Move(direction * Time.deltaTime);
    }
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
        if (groundedPlayer && verticalVelocity < 0)
    private void HandleInput()
    {
        float horizontalInput = moveAction.ReadValue<Vector2>().x;
        // Slide
        if (slideAction.triggered && controller.isGrounded && !isSliding)
            StartCoroutine(SlideRoutine());

        // Flip
        if (flipAction.triggered && controller.isGrounded && !isSliding)
            animator.SetTrigger("Flip");

        direction.x = xDelta;
        direction.z = forwardSpeed;

        controller.Move(direction * Time.deltaTime);
            verticalVelocity = jumpForce;
        
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
        else
        
        if (Mathf.Abs(horizontalInput) < 0.1f)
        {
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

        // Animator
        anim.SetBool("IsGrounded", groundedPlayer);
        anim.SetFloat("Speed", PlayerSpeed);
    }
}
