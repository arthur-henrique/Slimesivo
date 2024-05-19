using PlayerEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsEVfxManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private Transform jumpSameSidePos;



    private void OnEnable()
    {
        EventsPlayer.JumpLeft += SpawnJumpParticles;
        EventsPlayer.JumpRight += SpawnJumpParticles;
        EventsPlayer.JumpSameSide += _ => SpawnJumpSameSideParticles();
        EventsPlayer.Damage += SpawnDamageParticles;
        EventsPlayer.ClearAllEventsvariables += ClearEventsReferences;
    }

    private void ClearEventsReferences()
    {
        EventsPlayer.JumpLeft -= SpawnJumpParticles;
        EventsPlayer.JumpRight -= SpawnJumpParticles;
        EventsPlayer.JumpSameSide -= _ => SpawnJumpParticles();
        EventsPlayer.ClearAllEventsvariables -= ClearEventsReferences;
        Debug.Log("chamou");
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
        Instantiate(jumpParticles, jumpSameSidePos.position, Quaternion.identity);
    }

    private void SpawnWallParticle()
    {

    }
    private void SpawnPosWallParticle()
    {

    }
}
