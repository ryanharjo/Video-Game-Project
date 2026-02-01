using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunner : MonoBehaviour
{
    
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    
    public float jumpForce = 6f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 direction; 
    private int currentLane = 1; 

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    
    private bool canSwitchLane = true;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void OnEnable() { moveAction.Enable(); jumpAction.Enable(); }
    void OnDisable() { moveAction.Disable(); jumpAction.Disable(); }

    void Update()
    {
        HandleInput();

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

        direction.x = xDelta;
        direction.z = forwardSpeed;

        controller.Move(direction * Time.deltaTime);
    }

    private void HandleInput()
    {
        float horizontalInput = moveAction.ReadValue<Vector2>().x;

        
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

        
        if (Mathf.Abs(horizontalInput) < 0.1f)
        {
            canSwitchLane = true;
        }
    }
}
