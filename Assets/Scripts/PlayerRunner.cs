using UnityEngine;

public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 8f;
    public float laneDistance = 2f;
    public float laneChangeSpeed = 8f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    public float gravity = -20f;

    [Header("Sliding")]
    public float slideDuration = 0.8f;
    public float slideCooldown = 1.2f;
    public float slideHeight = 1.0f;   // height while sliding
    private float normalHeight = 2.0f; // default capsule height
    private float slideTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isSliding = false;

    private CharacterController cc;
    private int targetLane = 1;
    private float verticalVelocity = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        normalHeight = cc.height;
    }

    void Update()
    {
        HandleLaneSwitching();
        HandleJump();
        HandleSlide();

        // forward movement
        Vector3 forward = transform.forward * forwardSpeed;

        // apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = forward + Vector3.up * verticalVelocity;
        cc.Move(move * Time.deltaTime);

        // smooth lane movement
        float desiredX = (targetLane - 1) * laneDistance;
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Lerp(transform.position.x, desiredX, Time.deltaTime * laneChangeSpeed);
        transform.position = newPos;
    }

    // -------------------------
    // LANE SWITCHING
    // -------------------------
    void HandleLaneSwitching()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            targetLane = Mathf.Max(0, targetLane - 1);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            targetLane = Mathf.Min(2, targetLane + 1);
    }

    // -------------------------
    // JUMP
    // -------------------------
    void HandleJump()
    {
        if (cc.isGrounded && !isSliding)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                verticalVelocity = jumpForce;
            }
            else
            {
                verticalVelocity = -1f; // small grounding force
            }
        }
    }

    // -------------------------
    // SLIDE
    // -------------------------
    void HandleSlide()
    {
        cooldownTimer -= Time.deltaTime;
        slideTimer -= Time.deltaTime;

        // Start sliding
        if (!isSliding && cooldownTimer <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                StartSlide();
            }
        }

        // End sliding
        if (isSliding && slideTimer <= 0f)
        {
            EndSlide();
        }
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        cooldownTimer = slideCooldown;

        // change collider height
        cc.height = slideHeight;
        cc.center = new Vector3(0, slideHeight * 0.5f, 0);

        // optional animation trigger:
        // animator.SetTrigger("Slide");
    }

    void EndSlide()
    {
        isSliding = false;
        cc.height = normalHeight;
        cc.center = new Vector3(0, normalHeight * 0.5f, 0);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            FindFirstObjectByType<GameManager>()?.GameOver();
        }
    }
}
