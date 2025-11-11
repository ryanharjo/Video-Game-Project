using UnityEngine;
using System.Collections;

public class PlayerThrow : MonoBehaviour
{
    [Header("References")]
    public Transform throwPoint; 
    public GameObject axePrefab;
    public Camera cam;
    public GameManager gameManager;

    [Header("Throw settings")]
    public float throwForce = 18f;
    public float arcUpwards = 1.2f; 
    public float cooldown = 1.0f; 
    public int maxThrows = 10;

    bool ready = true;
    int throwsLeft;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        throwsLeft = maxThrows;
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && ready && throwsLeft > 0 && !gameManager.isPaused)
        {
            ThrowAxe();
        }
    }

    void ThrowAxe()
    {
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, throwPoint.position.y, 0));
        float enter;
        Vector3 aimPoint;
        if (plane.Raycast(ray, out enter))
            aimPoint = ray.GetPoint(enter);
        else
            aimPoint = throwPoint.position + cam.transform.forward * 10f;

        Vector3 dir = (aimPoint - throwPoint.position).normalized;

        GameObject axeObj = Instantiate(axePrefab, throwPoint.position, Quaternion.LookRotation(dir));
        Rigidbody rb = axeObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 force = dir * throwForce + Vector3.up * arcUpwards;
            rb.AddForce(force, ForceMode.VelocityChange);
            rb.angularVelocity = new Vector3(5f, 0f, 10f); 
        }

        throwsLeft--;
        gameManager.UpdateThrowsUI(throwsLeft);
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        ready = false;
        gameManager.ShowCooldown(cooldown);
        yield return new WaitForSeconds(cooldown);
        ready = true;
    }

    
    public void RefillThrows(int amount)
    {
        throwsLeft = Mathf.Clamp(throwsLeft + amount, 0, maxThrows);
        gameManager.UpdateThrowsUI(throwsLeft);
    }
}
