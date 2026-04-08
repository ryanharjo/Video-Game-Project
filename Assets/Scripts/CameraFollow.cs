using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 5, -7); // Adjust for height and distance
    public float laneSmoothTime = 0.1f;            // Faster response for lane switches

    private float xVelocity = 0.0f;

    void LateUpdate()
    {
        if (!player) return;

        // 1. Determine the target position
        // We take the player's current Z (forward) and Y (up), but handle X specially
        Vector3 targetPos = player.position + offset;

        // 2. Smoothly follow the lane change (X-axis) 
        // This prevents the camera from "snapping" too hard when switching lanes
        float newX = Mathf.SmoothDamp(transform.position.x, targetPos.x, ref xVelocity, laneSmoothTime);

        // 3. Apply the position
        // Z is constant relative to the player to ensure the "Endless" feel
        transform.position = new Vector3(newX, targetPos.y, targetPos.z);

        // 4. Always look slightly ahead of the player
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
