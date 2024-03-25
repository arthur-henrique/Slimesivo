using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speedFollowPlayer;


    private void FixedUpdate()
    {
        if(gameObject.transform.position.y < player.transform.position.y)
        {
            gameObject.transform.position = new Vector3(0, Mathf.Lerp(gameObject.transform.position.y, player.transform.position.y, speedFollowPlayer), 0);
        }
        
    }
    public void MoveCameraToRespawn()
    {
        gameObject.transform.position = new Vector3(0, Mathf.Lerp(gameObject.transform.position.y, player.transform.position.y, speedFollowPlayer), 0);
    }
}
