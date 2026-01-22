using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 6f;
    public float gravity = -20f;

    private CharacterController controller;
    private Animator anim;

    private float verticalVelocity;
    private int currentLane = 0;

    private float moveInput; // from new input system

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        anim.SetFloat("Speed", 1f); // always running
    }

    void Update()
    {
        Vector3 move = Vector3.forward * forwardSpeed;

        // Ground check
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            anim.SetBool("IsGrounded", true);
        }
        else
        {
            anim.SetBool("IsGrounded", false);
        }

        // Gravity
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        // Lane movement from new input
        float targetX = currentLane * laneDistance;
        float deltaX = targetX - transform.position.x;
        move.x = deltaX * laneSwitchSpeed;

        controller.Move(move * Time.deltaTime);
    }

    // Called by PlayerInput (Unity Events)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;

        if (moveInput > 0.5f)
            ChangeLane(1);
        else if (moveInput < -0.5f)
            ChangeLane(-1);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (controller.isGrounded)
        {
            verticalVelocity = jumpForce;
            anim.SetTrigger("Jump");
        }
    }

    void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }
}
