using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 5, -8); // Adjust for height and distance
    public float laneSmoothTime = 0.08f;            // Faster response for lane switches

    private float xVelocity = 0.0f;

    void LateUpdate()
    {
        if (!player) return;

        // Keep camera from following jump height too aggressively
        Vector3 targetPos = new Vector3(player.position.x, offset.y, player.position.z + offset.z);

        // Smooth lane switching
        float newX = Mathf.SmoothDamp(transform.position.x, targetPos.x, ref xVelocity, laneSmoothTime);

        // Smooth forward movement
        float newZ = Mathf.Lerp(transform.position.z, targetPos.z, 5f * Time.deltaTime);

        // Smooth height
        float newY = Mathf.Lerp(transform.position.y, targetPos.y, 3f * Time.deltaTime);

        transform.position = new Vector3(newX, newY, newZ);

        // Fixed viewing angle
        transform.rotation = Quaternion.Euler(15f, 0f, 0f);
    }
}
