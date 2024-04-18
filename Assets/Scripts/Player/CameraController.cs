using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speedFollowPlayer;
    [SerializeField] private CinemachineVirtualCamera vCam;
    private Vector3 startOffset;
    private float startDeadzone;
    [SerializeField] private float newDeadzone;
    [SerializeField] private float newOffset = 0;

    private void Start()
    {
         startOffset = vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        startDeadzone = vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight;
    }
    private void FixedUpdate()
    {
        if(gameObject.transform.position.y < player.transform.position.y)
        {
            gameObject.transform.position = new Vector3(0, Mathf.Lerp(gameObject.transform.position.y, player.transform.position.y, speedFollowPlayer), 0);
        }
        
    }
    public void MoveCameraToRespawn(float positionToMove)
    {
        gameObject.transform.position = new Vector3(0,positionToMove, 0);
        vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = newOffset;
        vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = newDeadzone;
    }

    public void CameraSettingsReset()
    {
        vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = startOffset;
        vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = startDeadzone;
    }
}
