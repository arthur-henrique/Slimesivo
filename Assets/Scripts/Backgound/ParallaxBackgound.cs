using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgound : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitySizeY;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitySizeY = texture.height/sprite.pixelsPerUnit;
    }
    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitySizeY)
        {
            
            float offSetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitySizeY;
            transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offSetPositionY);
        }
    }
}
