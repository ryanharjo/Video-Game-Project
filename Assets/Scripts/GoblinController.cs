using UnityEngine;

public class GoblinController : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    public float jumpHeight = 2.0f;
    public float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

       
        move = Vector3.ClampMagnitude(move, 1f);

        
        controller.Move(transform.TransformDirection(move) * playerSpeed * Time.deltaTime);

        
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
