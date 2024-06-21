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
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private AudioClip[] hitSounds;

    private bool inCooldownJumpSound;



    private void OnEnable()
    {
        EventsPlayer.JumpLeft += SpawnJumpParticles;
        EventsPlayer.JumpRight += SpawnJumpParticles;
        EventsPlayer.JumpSameSide += _ => SpawnJumpSameSideParticles();
        EventsPlayer.Damage += SpawnDamageParticles;
    }

    private void ClearEventsReferences()
    {
        EventsPlayer.JumpLeft -= SpawnJumpParticles;
        EventsPlayer.JumpRight -= SpawnJumpParticles;
        EventsPlayer.JumpSameSide -= _ => SpawnJumpParticles();
        EventsPlayer.Damage -= SpawnDamageParticles;
    }

    private void OnDisable()
    {
        ClearEventsReferences();
    }
    private void SpawnJumpParticles()
    {
        Instantiate(jumpParticles, transform.position, Quaternion.identity);
        if (!inCooldownJumpSound)
        {
            SoundFXManager.Instance.PlayRandomSoundFXClip(jumpSounds, transform, 1f);
            inCooldownJumpSound = true;
            StartCoroutine(CooldownTimerToPlayOtherSound());
        }
        
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
            if (!inCooldownJumpSound)
            {
                SoundFXManager.Instance.PlayRandomSoundFXClip(jumpSounds, transform, 1f);
                inCooldownJumpSound = true;
                StartCoroutine(CooldownTimerToPlayOtherSound());
            }

        }


    }

    IEnumerator CooldownTimerToPlayOtherSound()
    {
        float getSoundLength = jumpSounds[0].length;
        yield return new WaitForSeconds(getSoundLength);
        inCooldownJumpSound = false;

    }
}
