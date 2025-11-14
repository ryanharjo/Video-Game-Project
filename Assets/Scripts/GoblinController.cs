using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    public float speed = 6f;
    public float turnSmoothSpeed = 0.1f;

    float turnSmoothVelocity;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
