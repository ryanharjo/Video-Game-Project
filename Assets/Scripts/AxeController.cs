using UnityEngine;

public class AxeController : MonoBehaviour
{
    Rigidbody rb;
    bool stuck = false;
    float destroyAfter = 30f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (stuck) return;
        bool isTarget = collision.gameObject.CompareTag("Target") || collision.gameObject.layer == LayerMask.NameToLayer("Target");
        if (isTarget)
        {
            StickIn(collision);           
            collision.gameObject.SendMessage("OnHitByAxe", new AxeHitInfo { axe = this, collision = collision }, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Destroy(rb);
            Destroy(gameObject, destroyAfter);
        }
    }

    void StickIn(Collision collision)
    {
        stuck = true;       
        ContactPoint cp = collision.contacts[0];       
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;       
        transform.position = cp.point + cp.normal * 0.01f;        
        transform.rotation = Quaternion.LookRotation(-cp.normal) * Quaternion.Euler(90f, 0f, 0f);        
        transform.SetParent(collision.transform);       
        Destroy(gameObject, destroyAfter);
    }
}


public class AxeHitInfo
{
    public AxeController axe;
    public Collision collision;
}



