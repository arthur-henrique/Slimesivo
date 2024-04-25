using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEditor;
using UnityEngine;

public class PlayerStatsTutorial : MonoBehaviour
{
    private Vector2 respawnPos;

    //Components
    private TouchManagerTutorial touchManagerScript;
    private PlayerTutorial playerScript;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private CameraController cameraController;
    [SerializeField] LayerMask damageableLayer;
    [HideInInspector] public bool isRepawning;
    private Vector3 startPlayerPosition;
 

    private void Start()
    {
        StartCoroutine("DelayStart");
    }
    IEnumerator DelayStart()
    {
        float timer = 0.2f;
        yield return new WaitForSeconds(timer);
        playerScript = GetComponent<PlayerTutorial>();
        touchManagerScript = GetComponent<TouchManagerTutorial>();
        startPlayerPosition = gameObject.transform.position;
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
        //if(vidasPlayer <0)
        playerScript.ResetVelocityPlayer();
        if (cameraController.CheckSpawnPosition())
        {
            if (touchManagerScript.IsFacingRight)
            {
                gameObject.transform.position = respawnPos;
            }
            else
            {
                gameObject.transform.position = respawnPos;
            }
        }
        else
        {
            gameObject.transform.position = startPlayerPosition;
        }
       
        cameraController.MoveCameraToRespawn(respawnPos.y);
    }
    public void RespawnPlayerSameSide()
    {
        playerScript.ResetVelocityPlayer();
        if (touchManagerScript.IsFacingRight)
        {
            gameObject.transform.position = respawnPos;           
        }
        else
        {
            gameObject.transform.position = respawnPos;
        }
        cameraController.MoveCameraToRespawn(respawnPos.y);
    }

    private bool DangerousRespawnPoint()
    {
        return Physics2D.OverlapCapsule(gameObject.transform.position, new Vector2(0.72f, 2.77f), CapsuleDirection2D.Vertical, 0f, damageableLayer);
    }
 



}
