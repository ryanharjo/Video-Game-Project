using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void Start()
    {
        // Calculate the initial offset based on editor positions
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Set camera position to player position + offset
        // We only update X and Z to keep the camera height (Y) steady if desired, 
        // or just follow everything:
        Vector3 newPos = player.position + offset;
        newPos.x = 0; // Optional: Keep camera centered in the lane (looks better)
        transform.position = newPos;
    }
}
