using UnityEngine;

public class SilverManeController : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        if (Time.timeScale == 0f) return; 

        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(moveX, 0, moveZ);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            TheOtherGameManager.Instance.GameOver();
        }
    }
}
