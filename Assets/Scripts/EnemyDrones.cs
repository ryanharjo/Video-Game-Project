using UnityEngine;

public class EnemyDrones : MonoBehaviour
{
    void Update()
    {
        
        if (transform.position.z < -15f)
        {
            Destroy(gameObject);
        }
    }
}
