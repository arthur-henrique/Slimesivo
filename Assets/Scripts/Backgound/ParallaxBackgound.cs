using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgound : MonoBehaviour
{
    [SerializeField][Range(0,1)] float parallaxEffectMultiplier = 0.5f; 
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * parallaxEffectMultiplier;
        lastCameraPosition = cameraTransform.position;
    }
}
