using PlayerEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsEVfxManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private Sprite wallParticle;
    [SerializeField] private Transform jumpSameSidePos;
      [SerializeField] private TouchManager touchManager;



    private void OnEnable()
    {
        EventsPlayer.JumpLeft += SpawnJumpParticles;
        EventsPlayer.JumpRight += SpawnJumpParticles;
        EventsPlayer.JumpSameSide += _ => SpawnJumpSameSideParticles();
        EventsPlayer.Damage += SpawnDamageParticles;
        EventsPlayer.WallStick += SpawnWallParticle;
    }

    private void ClearEventsReferences()
    {
        EventsPlayer.JumpLeft -= SpawnJumpParticles;
        EventsPlayer.JumpRight -= SpawnJumpParticles;
        EventsPlayer.JumpSameSide -= _ => SpawnJumpParticles();
        EventsPlayer.Damage -= SpawnDamageParticles;
        EventsPlayer.WallStick -= SpawnWallParticle;
    }

    private void OnDisable()
    {
        ClearEventsReferences();
    }
    private void SpawnJumpParticles()
    {
        Instantiate(jumpParticles, transform.position, Quaternion.identity);
    }
    private void SpawnDamageParticles(Collider2D collider, Transform player)
    {
        Instantiate(damageParticles, player.position, Quaternion.identity);
    }
    private void SpawnJumpSameSideParticles()
    {
        if(jumpSameSidePos != null)
        {
            Instantiate(jumpParticles, jumpSameSidePos.position, Quaternion.identity);
        }
       
    }

    private void SpawnWallParticle(Vector3 spawnPos)
    {
        if (Player.Instance.canSpawnWallVfx)
        {
            if (touchManager.IsFacingRight)
            {
                Instantiate(wallParticle, spawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(wallParticle, spawnPos, Quaternion.Euler(0,180,0));
            }
            
        }
       
    }
}
