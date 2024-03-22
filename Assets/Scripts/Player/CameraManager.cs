using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]private float timer;
    [SerializeField] private float speedOfCamera;
    [SerializeField] private Player player;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera c_VirtualCamera;

    private bool canMoveUp;
    // Update is called once per frame

    private void FixedUpdate()
    {
        Vector2 targetPos = transform.position * 5;
        if (canMoveUp)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, speedOfCamera * Time.deltaTime);
        }
       
        
    }
    public void MoveCameraUp()
    {
        Debug.Log("Up");
        c_VirtualCamera.m_Follow = gameObject.transform;
        canMoveUp = true;
    }
    public void StopCameraUp()
    {
        canMoveUp = false;
    }
}
