using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunner : MonoBehaviour
{
    // Movement input
    private float horizontal;
    private float vertical;

    // Jump variables
    private float jumpTime = 1.0f;
    private float jumpTimer;
    private bool hasJumped = false;
    private bool canJump = true;

    // Optional speed property (for powerups)
    public float PlayerSpeed { get; set; } = 10f;

    // Physics
    public float gravity = -20f;
    public float jumpForce = 6f;

    private CharacterController controller;
    private Animator anim;

    private float verticalVelocity;
    private bool groundedPlayer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        // Initialize jump timer
        jumpTimer = 0.0f;
    }

    // New Input System - Move
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }

    // New Input System - Jump
    public void Jump(InputAction.CallbackContext context)
    {
        hasJumped = context.ReadValueAsButton();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        // Forward endless runner movement + horizontal input
        Vector3 move = new Vector3(horizontal, 0, PlayerSpeed);

        // Stick to ground
        if (groundedPlayer && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        // Jump logic (replaces Input.GetButtonDown)
        if (hasJumped && canJump && groundedPlayer)
        {
            verticalVelocity = jumpForce;
            jumpTimer = jumpTime;
            canJump = false;
            groundedPlayer = false;

            anim.SetTrigger("Jump");
        }

        // Jump cooldown timer
        if (canJump == false)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (jumpTimer <= 0)
        {
            jumpTimer = 0;
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        // Gravity
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        // Apply movement
        controller.Move(move * Time.deltaTime);

        // Animator
        anim.SetBool("IsGrounded", groundedPlayer);
        anim.SetFloat("Speed", PlayerSpeed);
    }
}
