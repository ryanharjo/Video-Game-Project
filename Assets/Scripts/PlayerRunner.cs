using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;
    private bool isFinished = false;

    [Header("Jump Settings")]
    public float jumpForce = 1.8f;
    public float gravity = -32f;
    public float fallMultiplier = 2f;
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    [Header("UI & Finish Settings")]
    public GameObject levelCompletePanel;
    public float uiDelay = 2.0f;



    private CharacterController controller;
    private Animator animator;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 mobileMoveInput;
    private bool mobileJumpInput;
    private Vector3 velocity;
    private int currentLane = 1;
    private bool canSwitchLane = true;
    private bool isDead = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.GameOver)
        {
            return;
        }

        if (!isFinished)
        {
            HandleInput();
            UpdateAnimator();
            HandleMovement();
        }
    }

    void HandleInput()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Mobile input override
        if (mobileMoveInput != Vector2.zero)
            input = mobileMoveInput;

        if (canSwitchLane)
        {
            if (input.x > 0.5f && currentLane < 2)
            {
                currentLane = Mathf.Clamp(currentLane + (input.x > 0.5f ? 1 : input.x < -0.5f ? -1 : 0), 0, 2);
                StartCoroutine(LaneSwitchCooldown());
            }
            else if (input.x < -0.5f && currentLane > 0)
            {
                currentLane--;
                StartCoroutine(LaneSwitchCooldown());
            }
        }
    }

    IEnumerator LaneSwitchCooldown()
    {
        canSwitchLane = false;
        yield return new WaitForSeconds(0.2f);
        canSwitchLane = true;
    }

    void HandleMovement()
    {
        float targetX = (currentLane - 1) * laneDistance;
        Vector3 currentPosition = transform.position;

        float newX = Mathf.MoveTowards(currentPosition.x, targetX, laneSwitchSpeed * Time.deltaTime);

        float xDelta = newX - currentPosition.x;

        // Gravity
        if (velocity.y < 0) 
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        }
        else if (velocity.y > 0 && !jumpAction.IsPressed())
        {
            velocity.y += gravity * (fallMultiplier * 0.5f) * Time.deltaTime;
        }
        else 
        {
            velocity.y += gravity * Time.deltaTime;
        }

        bool jumpPressed = jumpAction.triggered || mobileJumpInput;

        if (controller.isGrounded && jumpBufferCounter > 0 && !isFinished)
        {
            if (velocity.y < 0)
                velocity.y = -1f;

            if (jumpPressed && !isFinished)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                jumpBufferCounter = 0;
                if (animator != null)
                    animator.SetTrigger("Jump");

                mobileJumpInput = false;
            }
        }

        if (isFinished)
        {
            velocity.y = 0;
        }

        if (jumpAction.triggered)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        float currentForwardMove = isFinished? 0f: forwardSpeed * Time.deltaTime;

        Vector3 moveVector = new Vector3(xDelta, velocity.y, currentForwardMove);

        controller.Move(moveVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine") && !isFinished)
        {
            StartCoroutine(CompleteLevelRoutine());
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle") && !isFinished)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        isFinished = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
            animator.SetBool("IsRunning", false);
        }

        velocity = Vector3.zero;

        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
    }

    void TriggerDeath()
    {
        isFinished = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
            animator.SetBool("IsRunning", false);
        }

        velocity = Vector3.zero;

        Debug.Log("Game Over!");
    }

    IEnumerator CompleteLevelRoutine()
    {
        isFinished = true;

        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
        }

        yield return new WaitForSeconds(uiDelay);

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("IsRunning", !isFinished);
            animator.SetBool("IsGrounded", controller.isGrounded);
        }
    }

    public void MoveInput(Vector2 value)
    {
        mobileMoveInput = value;
    }

    public void JumpInput(bool value)
    {
        mobileJumpInput = value;
    }
}
