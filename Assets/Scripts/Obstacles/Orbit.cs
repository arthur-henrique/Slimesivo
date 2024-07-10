using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform anchor; // The anchor around which the object will orbit
    public float orbitRadius = 2f; // The radius of the orbit
    public float orbitSpeed = 1f; // The speed of the orbit

    private float angle; // The current angle in radians

    void Start()
    {
        // Randomize the starting angle for variety
        //angle = Random.Range(0f, Mathf.PI * 2f);

        // Initialize the starting angle based on the current position relative to the anchor
        Vector2 offset = transform.position - anchor.position;
        angle = Mathf.Atan2(offset.y, offset.x);
    }

    void Update()
    {
        // Increment the angle based on the orbit speed and time
        angle += orbitSpeed * Time.deltaTime;

        // Calculate the new position using trigonometric functions
        float x = anchor.position.x + Mathf.Cos(angle) * orbitRadius;
        float y = anchor.position.y + Mathf.Sin(angle) * orbitRadius;

        // Update the position of the orbiting object
        transform.position = new Vector2(x, y);
    }
}
