using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    public float minSpeed = 1f; // Minimum rotation speed
    public float maxSpeed = 3f; // Maximum rotation speed
    public float angle = 45f; // Maximum angle for the back-and-forth rotation

    private float speed;
    private float currentAngle;
    private bool rotatingClockwise;
    private float directionMultiplier;

    void Start()
    {
        // Randomize the rotation speed and initial direction
        speed = Random.Range(minSpeed, maxSpeed);
        rotatingClockwise = Random.value > 0.5f;
        directionMultiplier = rotatingClockwise ? 1f : -1f;
    }

    void Update()
    {
        // Calculate the rotation based on speed and time
        currentAngle += directionMultiplier * speed * Time.deltaTime;

        // Apply an ease-in effect when changing direction
        float t = Mathf.PingPong(Time.time * speed, 1f); // t goes from 0 to 1 and back to 0
        float easeFactor = Mathf.SmoothStep(0f, 1f, t); // SmoothStep creates an ease-in-out effect
        float targetAngle = Mathf.Lerp(-angle, angle, easeFactor);

        // Update the rotation of the sprite
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);

        // Reverse direction when reaching the angle limit
        if (Mathf.Abs(targetAngle) >= angle)
        {
            directionMultiplier *= -1;
        }
    }
}
