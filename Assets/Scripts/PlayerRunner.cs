using UnityEngine;

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
    private int currentLane = 0; // -1 = left, 0 = middle, 1 = right

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
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

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                anim.SetTrigger("Jump");
            }
        }
        else
        {
            anim.SetBool("IsGrounded", false);
        }

        // Gravity
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        // Lane switching
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ChangeLane(1);

        float targetX = currentLane * laneDistance;
        float deltaX = targetX - transform.position.x;

        move.x = deltaX * laneSwitchSpeed;

        controller.Move(move * Time.deltaTime);

        // Animation speed
        anim.SetFloat("Speed", forwardSpeed);
    }

    void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }
}
