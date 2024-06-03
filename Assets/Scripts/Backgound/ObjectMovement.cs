using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public float maxMoveDistance = 20f; // Maximum distance to move in either direction
    public float rotationSpeed = 20f; // Speed of rotation
    public float rotationChangeSpeed = 1.0f; // Speed at which rotation speed changes
    public AnimationCurve easingCurve; // Define an animation curve for easing

    private Vector3 startPosition;
    private float targetRotationSpeed;
    private bool isChangingRotation;
    private bool moveUp; // Flag to indicate initial direction of movement

    void Start()
    {
        startPosition = transform.position;
        // Randomly determine initial direction
        moveUp = Random.value > 0.5f; // 50% chance of moving up
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * scrollSpeed, maxMoveDistance);
        float easedT = easingCurve.Evaluate(t / maxMoveDistance); // Use easing curve for smoother movement
        float directionMultiplier = moveUp ? 1f : -1f;
        transform.position = startPosition + Vector3.up * directionMultiplier * easedT;

        // Check if the rotation angle exceeds a threshold in either direction
        if (Quaternion.Angle(transform.rotation, Quaternion.identity) > 35 && Quaternion.Angle(transform.rotation, Quaternion.identity) < 180)
        {
            // Start changing rotation speed
            if (!isChangingRotation)
            {
                isChangingRotation = true;
                targetRotationSpeed = -rotationSpeed;
            }
        }
        else
        {
            // Stop changing rotation speed
            if (isChangingRotation)
            {
                isChangingRotation = false;
                targetRotationSpeed = rotationSpeed;
            }
        }

        // Smoothly change rotation speed
        if (isChangingRotation)
        {
            rotationSpeed = Mathf.Lerp(rotationSpeed, targetRotationSpeed, rotationChangeSpeed * Time.deltaTime);
        }

        // Add rotation
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }



}
