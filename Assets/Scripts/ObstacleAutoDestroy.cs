using UnityEngine;

public class ObstacleAutoDestroy : MonoBehaviour
{
    public float destroyZ = -10f; // z position where obstacle is destroyed
    public float moveSpeed = 0f; // if zero, obstacles are static and player moves forward
    void Update()
    {
        if (moveSpeed > 0f)
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (transform.position.z < destroyZ)
            Destroy(gameObject);
    }
}
