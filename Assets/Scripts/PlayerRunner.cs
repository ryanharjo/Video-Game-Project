using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    public float gravity = -20f;
    private bool isFinished = false;

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
        if (GameManager.Instance.currentState == GameManager.GameState.GameOver) return;

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

        if (mobileMoveInput != Vector2.zero)
            input = mobileMoveInput;

        if (canSwitchLane)
        {
            if (input.x > 0.5f && currentLane < 2)
            {
                currentLane++;
                StartCoroutine(LaneSwitchCooldown());
            }
            else if (input.x < -0.5f && currentLane > 0)
            {
                currentLane--;
                StartCoroutine(LaneSwitchCooldown());
            }
        }

        if (jumpAction.triggered || mobileJumpInput && controller.isGrounded)
        {
            velocity.y = jumpForce;

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }

            mobileJumpInput = false;
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
        // Calculate Horizontal Movement (Lanes)
        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);
        float xDelta = newX - transform.position.x;

        // Apply a constant small downward force to stay grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }



        // Calculate Forward Movement
        float currentForwardMove = isFinished ? 0f : forwardSpeed * Time.deltaTime;

        // Combine into Move Vector
        Vector3 moveVector = new Vector3(xDelta, velocity.y * Time.deltaTime, currentForwardMove);

        controller.Move(moveVector);
    }

    // --- Collision & UI Logic (Unchanged) ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine") && !isFinished)
            StartCoroutine(CompleteLevelRoutine());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle") && !isFinished)
            Die();
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
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
    }

    IEnumerator CompleteLevelRoutine()
    {
        isFinished = true;
        if (animator != null) animator.SetBool("IsRunning", false);
        yield return new WaitForSeconds(uiDelay);
        if (levelCompletePanel != null) levelCompletePanel.SetActive(true);
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("IsRunning", !isFinished);
            bool grounded = controller.isGrounded;
            animator.SetBool("IsGrounded", grounded);
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
