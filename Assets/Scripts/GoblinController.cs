using UnityEngine;

public class GoblinController : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 200f;
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * v + transform.right * h;
        controller.Move(move * speed * Time.deltaTime);
    }
}
