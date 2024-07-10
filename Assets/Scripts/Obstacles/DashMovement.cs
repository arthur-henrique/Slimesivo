using UnityEngine;
using System.Collections;

public class DashMovement : MonoBehaviour
{
    public Vector2[] positions; // Array of positions to loop through
    public float waitTime = 2f; // Time to wait at each position
    public float dashTime = 0.5f; // Time taken to dash to the next position
    public float scaleDownTime = 0.25f; // Time taken to scale down before dashing
    public float scaleUpTime = 0.25f; // Time taken to scale up after dashing
    public Vector2 dashScale = new Vector2(0.5f, 0.5f); // Scale during the dash
    public Vector2 overshootScale = new Vector2(1.2f, 1.2f); // Scale to overshoot to after dashing

    private int currentIndex = 0;
    private Vector2 originalScale; // Store the original scale
    private CircleCollider2D circleCollider; // Reference to the CircleCollider2D

    void Start()
    {
        // Store the original scale of the game object
        originalScale = transform.localScale;

        // Get the CircleCollider2D component
        circleCollider = GetComponent<CircleCollider2D>();

        // Start the coroutine to handle the movement
        StartCoroutine(DashToPositions());
    }

    IEnumerator DashToPositions()
    {
        while (true)
        {
            // Wait at the current position for the specified wait time
            yield return new WaitForSeconds(waitTime);

            // Start scaling down before the dash
            yield return StartCoroutine(ScaleDown());

            // Deactivate the collider
            circleCollider.enabled = false;

            // Get the next position in the array
            Vector2 nextPosition = positions[currentIndex];

            // Smoothly dash to the next position
            yield return StartCoroutine(DashToPosition(nextPosition));

            // Update the index for the next position, looping back to start if necessary
            currentIndex = (currentIndex + 1) % positions.Length;

            // Scale up to overshoot size and then back to the original size after dashing
            yield return StartCoroutine(ScaleUpWithOvershoot());

            // Reactivate the collider
            circleCollider.enabled = true;
        }
    }

    IEnumerator ScaleDown()
    {
        float elapsedTime = 0f;

        while (elapsedTime < scaleDownTime)
        {
            // Smoothly interpolate the scale down
            transform.localScale = Vector2.Lerp(originalScale, dashScale, elapsedTime / scaleDownTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly the dash scale
        transform.localScale = dashScale;
    }

    IEnumerator DashToPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < dashTime)
        {
            // Smoothly interpolate between the start and target positions
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / dashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        transform.position = targetPosition;
    }

    IEnumerator ScaleUpWithOvershoot()
    {
        float elapsedTime = 0f;

        // First phase: scale up to the overshoot size
        while (elapsedTime < scaleUpTime)
        {
            transform.localScale = Vector2.Lerp(dashScale, overshootScale, elapsedTime / scaleUpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly the overshoot scale
        transform.localScale = overshootScale;

        elapsedTime = 0f;

        // Second phase: scale back to the original size
        while (elapsedTime < scaleUpTime)
        {
            transform.localScale = Vector2.Lerp(overshootScale, originalScale, elapsedTime / scaleUpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly the original scale
        transform.localScale = originalScale;
    }
}
