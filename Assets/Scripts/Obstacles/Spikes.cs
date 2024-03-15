using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private PlayerStats playerStatsScript;
    private void OnTriggerEnter2D(Collider2D other)
    {
        playerStatsScript = other.GetComponentInParent<PlayerStats>();
        playerStatsScript.RespawnPlayer();
    }
}
