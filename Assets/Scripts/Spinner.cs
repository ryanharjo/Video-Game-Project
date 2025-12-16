using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float degreesPerSecond = 90f;

    void Update()
    {
        transform.Rotate(0, degreesPerSecond * Time.deltaTime, 0);
    }
}
