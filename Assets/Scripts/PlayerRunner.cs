using UnityEngine;

public class PlayerRunner : MonoBehaviour
{
    // Movement speeds
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float dodgeSpeed = 10f;

    // Jump / gravity
    public float jumpForce = 8f;
    public float gravity = -20f;
    private float verticalVelocity;

    // Slide settings
    public float slideDuration = 0.7f;
    private bool isSliding = false;
    private float slideTimer;
    private float originalHeight;
    private Vector3 originalCenter;

    private CharacterController controller;
    private int targetLane = 1;  // 0 = left, 1 = middle, 2 = right

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        originalCenter = controller.center;
    }

    void Update()
    {
        // ==============================
        // 1. Lane Movement (Left/Right)
        // ==============================
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ChangeLane(1);

        float targetX = (targetLane - 1) * laneDistance;


        // ==============================
        // 2. Jump Input
        // ==============================
        if (controller.isGrounded)
        {
            if (!isSliding && Input.GetKeyDown(KeyCode.UpArrow))
                verticalVelocity = jumpForce;
        }


        // ==============================
        // 3. Slide Input
        // ==============================
        if (!isSliding && controller.isGrounded && Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartSlide();
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
                StopSlide();
        }


        // ==============================
        // 4. Apply Gravity (always)
        // ==============================
        if (!controller.isGrounded)
            verticalVelocity += gravity * Time.deltaTime;
        else if (verticalVelocity < -2f)
            verticalVelocity = -2f;


        // ==============================
        // 5. Build Final Move Vector
        // ==============================
        Vector3 move = Vector3.forward * forwardSpeed;  // forward run

        // Horizontal lane movement
        float newX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * dodgeSpeed);
        move.x = (newX - transform.position.x) / Time.deltaTime;

        // Vertical (jump or slide)
        move.y = verticalVelocity;


        // ==============================
        // 6. Apply Move
        // ==============================
        controller.Move(move * Time.deltaTime);
    }


    // Change which lane you're in
    void ChangeLane(int direction)
    {
        targetLane += direction;
        targetLane = Mathf.Clamp(targetLane, 0, 2);
    }


    // ==============================
    // Slide Functions
    // ==============================
    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        controller.height = originalHeight / 2f;
        controller.center = new Vector3(originalCenter.x, originalCenter.y / 2f, originalCenter.z);
    }

    void StopSlide()
    {
        isSliding = false;
        controller.height = originalHeight;
        controller.center = originalCenter;
    }
}
