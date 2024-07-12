using UnityEngine;

public class CenterGameObject : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        CenterObject();
    }

    void CenterObject()
    {
        // Get the screen's center position in screen coordinates (pixels)
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Convert the screen center to world coordinates
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);

        // Set the Z position to match the game object's original Z position
        worldCenter.z = transform.position.z;

        // Set the game object's position to the world center
        transform.position = worldCenter;
    }
}
