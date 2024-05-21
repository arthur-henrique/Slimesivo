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
    private float newDeadzone;
    private float newOffset = 0;
    [SerializeField] private float offsetFollowCamera;
    Vector2 playerLastPos;

    private Vector2 spawnPosition;

    private void Start()
    {
        StartCoroutine(DelayStart());
    }
    IEnumerator DelayStart()
    {
        float timer = 0.2f;
        yield return new WaitForSeconds(timer);
        startOffset = vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        startDeadzone = vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight;
        spawnPosition = vCam.transform.position;
    }

    public bool CheckSpawnPosition()
    {
        return gameObject.transform.position.y > spawnPosition.y;
    }
    private void Update()
    {
        MoveCamera();
        
    }
    private void MoveCamera()
    {
        
        if (player.transform.position.y > playerLastPos.y)
        {
            playerLastPos = player.transform.position; 
            gameObject.transform.position = new Vector3(0, Mathf.Lerp(gameObject.transform.position.y + offsetFollowCamera, player.transform.position.y, speedFollowPlayer), 0);
        }
    }
    public void MoveCameraToRespawn(float positionToMove)
    {
        Debug.Log(CheckSpawnPosition());
        
        if (CheckSpawnPosition())
        {
            gameObject.transform.position = new Vector3(0, positionToMove, 0);
            vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = newOffset;
            vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = newDeadzone;
        }
    }

    public void CameraSettingsReset()
    {
        vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = startOffset;
        vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = startDeadzone;
    }
}
