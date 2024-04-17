using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEditor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private Vector2 respawnPos;

    //Components
    private TouchManager touchManagerScript;
    private Player playerScript;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private CameraController cameraController;
    [SerializeField] LayerMask damageableLayer;
    [HideInInspector] public bool isRepawning;
 

    private void Awake()
    {
        playerScript = GetComponent<Player>();
        touchManagerScript= GetComponent<TouchManager>(); 
       
    }

    public void SaveCurrentPlayerPos()
    {
            if (!DangerousRespawnPoint())
          {

            if (playerScript.IsOnGround() || playerScript.IsWalled())
            {
                respawnPos = gameObject.transform.position;
            }
          }
        
            
        
      
    }


    public void RespawnPlayer()
    {
        
        isRepawning = true;
        cameraController.MoveCameraToRespawn();
        //if(vidasPlayer <0)
        playerScript.ResetVelocityPlayer();
        if (touchManagerScript.isFacingRight)
        {
            //touchManagerScript.rightCounter = 0;
            //touchManagerScript.leftCounter = 1;
            gameObject.transform.position = respawnPos;
            //touchManagerScript.isFacingRight = false;
            


        }
        else
        {
            //touchManagerScript.rightCounter = 1;
            //touchManagerScript.leftCounter = 0;
            gameObject.transform.position = respawnPos;
            //touchManagerScript.isFacingRight = true;
           

        }
    }
    public void RespawnPlayerSameSide()
    {
        cameraController.MoveCameraToRespawn();
        playerScript.ResetVelocityPlayer();
        if (touchManagerScript.isFacingRight)
        {
            touchManagerScript.rightCounter = 1;
            touchManagerScript.leftCounter = 0;
            gameObject.transform.position = respawnPos;
            touchManagerScript.isFacingRight = true;
            


        }
        else
        {
            touchManagerScript.rightCounter = 0;
            touchManagerScript.leftCounter = 1;
            gameObject.transform.position = respawnPos;
            touchManagerScript.isFacingRight = false;
          

        }
    }

    private bool DangerousRespawnPoint()
    {
        return Physics2D.OverlapCapsule(gameObject.transform.position, new Vector2(0.72f, 2.77f), CapsuleDirection2D.Vertical, 0f, damageableLayer);
    }
 



}
