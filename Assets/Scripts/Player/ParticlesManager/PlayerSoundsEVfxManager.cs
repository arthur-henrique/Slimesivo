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
    [SerializeField] private AudioClip hitSounds;
    [SerializeField] private AudioClip walledSound;

    private bool inCooldownSound;


    private void OnEnable()
    {
        EventsPlayer.JumpLeft += SpawnJumpParticles;
        EventsPlayer.JumpRight += SpawnJumpParticles;
        EventsPlayer.JumpSameSide += _ => SpawnJumpSameSideParticles();
        EventsPlayer.Damage += SpawnDamageParticles;
        EventsPlayer.WallStick += PlayWalledSound;
    }

    private void ClearEventsReferences()
    {
        EventsPlayer.JumpLeft -= SpawnJumpParticles;
        EventsPlayer.JumpRight -= SpawnJumpParticles;
        EventsPlayer.JumpSameSide -= _ => SpawnJumpParticles();
        EventsPlayer.Damage -= SpawnDamageParticles;
        EventsPlayer.WallStick -= PlayWalledSound;
    }

    private void OnDisable()
    {
        ClearEventsReferences();
    }
    private void SpawnJumpParticles()
    {
        Instantiate(jumpParticles, transform.position, Quaternion.identity);
        SoundFXManager.Instance.PlayRandomSoundFXClip(jumpSounds, transform, 1f);
    }
    private void SpawnDamageParticles(Collider2D collider, Transform player)
    {
        Instantiate(damageParticles, player.position, Quaternion.identity);
        if (!inCooldownSound)
        {
            SoundFXManager.Instance.PlaySoundFXClip(hitSounds, transform, 1f);
            inCooldownSound = true;
            StartCoroutine(CooldownTimerToPlayOtherSound(hitSounds));
        }

    }
    private void SpawnJumpSameSideParticles()
    {
        if(jumpSameSidePos != null)
        {
            Instantiate(jumpParticles, jumpSameSidePos.position, Quaternion.identity);
            if (!inCooldownSound)
            {
                SoundFXManager.Instance.PlayRandomSoundFXClip(jumpSounds, transform, 1f);
                inCooldownSound = true;
                StartCoroutine(CooldownTimerToPlayOtherSound(jumpSounds[0]));
            }

        }


    }

    IEnumerator CooldownTimerToPlayOtherSound(AudioClip audioClip)
    {
        float getSoundLength = audioClip.length;
        yield return new WaitForSeconds(getSoundLength);
        inCooldownSound = false;

    }


    private void PlayWalledSound(bool validWall)
    {
        if (validWall)
        {
            SoundFXManager.Instance.PlaySoundFXClip(walledSound, transform, 1f);
        }
        
    }
}
