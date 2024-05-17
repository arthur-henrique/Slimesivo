using PlayerEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsEVfxManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem damageParticles;



    private void OnEnable()
    {
        EventsPlayer.JumpLeft += SpawnJumpParticles;
        EventsPlayer.JumpRight += SpawnJumpParticles;
        EventsPlayer.JumpSameSide += _ => SpawnJumpParticles();
        EventsPlayer.Damage += SpawnDamageParticles;
    }
    private void OnDisable()
    {
        EventsPlayer.JumpLeft -= SpawnJumpParticles;
        EventsPlayer.JumpRight -= SpawnJumpParticles;
        EventsPlayer.JumpSameSide -= _ => SpawnJumpParticles();
    }
    private void SpawnJumpParticles()
    {
        Instantiate(jumpParticles, transform.position, Quaternion.identity);
    }
    private void SpawnDamageParticles(Collider2D collider, Transform player)
    {
        Instantiate(damageParticles, player.position, Quaternion.identity);
    }
}
