using UnityEngine;

public class PlayerRunner : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float horizontalSpeed = 7f;

    void Update()
    {
        // Constant forward movement
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Left / Right movement
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * horizontalSpeed * Time.deltaTime);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 500f);
    }
}
