using UnityEngine;

public class ObstacleDisable : MonoBehaviour
{
    public float disableDistance = 20f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player.position.z - transform.position.z > disableDistance)
        {
            gameObject.SetActive(false);
        }
    }
}
