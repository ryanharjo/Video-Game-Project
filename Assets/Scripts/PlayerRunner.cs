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

    [Header("UI & Finish Settings")]
    public GameObject levelCompletePanel;
    public float uiDelay = 2.0f;

    private CharacterController controller;
    private Animator animator;
    private PlayerInput playerInput;
    private InputAction moveAction;

    private Vector2 mobileMoveInput;
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

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    void OnEnable() => moveAction.Enable();
    void OnDisable() => moveAction.Disable();

    void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.GameOver) 

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
        if (controller.isGrounded)
            velocity.y = -1f;
        else
            velocity.y += -9.81f * Time.deltaTime; // Simple gravity in case they fall off an edge

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
            animator.SetBool("IsGrounded", controller.isGrounded);
        }
    }

    public void MoveInput(Vector2 value) => mobileMoveInput = value;
}
