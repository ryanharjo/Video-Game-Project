using UnityEngine;

public class ColorSwitcher : MonoBehaviour
{
    public Color colorA = Color.green;
    public Color colorB = Color.red;
    public float speed = 2.0f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calculate the ping-pong value (0 to 1)
        float t = Mathf.PingPong(Time.time * speed, 1.0f);

        // Interpolate between colors
        Color lerpedColor = Color.Lerp(colorA, colorB, t);

        // Apply to both base color and emission
        rend.material.SetColor("_BaseColor", lerpedColor); // Use "_Color" for older shaders
        rend.material.SetColor("_EmissionColor", lerpedColor);

        // Optional: Keep the bloom effect strong
        DynamicGI.SetEmissive(rend, lerpedColor);
    }
}
