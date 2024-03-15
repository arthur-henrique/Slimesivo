using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private Vector2 respawnPos;
    private Player playerScript;

    private void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    public void SaveCurrentPlayerPos()
    {
        if (playerScript.IsOnGround() || playerScript.IsWalled())
        {
           respawnPos = gameObject.transform.position;
           
        }
    }


    public void RespawnPlayer()
    {
        gameObject.transform.position = respawnPos;
        //if(vidasPlayer <0)
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.position = respawnPos;
        }
       
    }
}
