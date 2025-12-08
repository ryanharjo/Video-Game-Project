using UnityEngine;

public class Ground : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float resetZ = -20f;
    public float startZ = 40f;

    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (transform.position.z < resetZ)
        {
            Vector3 pos = transform.position;
            pos.z = startZ;
            transform.position = pos;
        }
    }
}
