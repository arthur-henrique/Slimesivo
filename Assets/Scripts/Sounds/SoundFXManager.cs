using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //Spawn in a game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //Assing the audio clip
        audioSource.clip = audioClip;

        //Assing the volume 
        audioSource.volume = volume;

        //Play sound
        audioSource.Play();

        //Get clip Length 
        float clipLength = audioSource.clip.length;

        //Destroy the clip after it is done playing 
        Destroy(audioSource.gameObject,clipLength);
    }
    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int randomSound = Random.Range(0, audioClip.Length);
        //Spawn in a game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //Assing the audio clip
        audioSource.clip = audioClip[randomSound];

        //Assing the volume 
        audioSource.volume = volume;

        //Play sound
        audioSource.Play();

        //Get clip Length 
        float clipLength = audioSource.clip.length;

        //Destroy the clip after it is done playing 
        Destroy(audioSource.gameObject, clipLength);
    }

}
