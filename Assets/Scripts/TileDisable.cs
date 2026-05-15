using UnityEngine;

public class TileDisable : MonoBehaviour
{
    public float disableDistance = 30f;

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
