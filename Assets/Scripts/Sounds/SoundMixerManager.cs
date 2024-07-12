using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider[] soundFXVolumeSliders;
    [SerializeField] private Slider[] musicVolumeSliders;
    [SerializeField] private TextMeshProUGUI[] soundFXVolumeTexts;
    [SerializeField] private TextMeshProUGUI[] musicVolumeTexts;

    private void Start()
    {
        SetElements();
    }


    public void SetElements()
    {
        // Load stored values or set default values
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float masterVolume = PlayerPrefs.GetFloat("masterVolume");
            SetMasterVolume(masterVolume);

        }
        else
        {
            PlayerPrefs.SetFloat("masterVolume", 1);
            SetMasterVolume(1);
        }

        if (PlayerPrefs.HasKey("soundFXVolume"))
        {
            float soundFXVolume = PlayerPrefs.GetFloat("soundFXVolume");
            SetSoundFXVolume(soundFXVolume);
            for (int i = 0; i < soundFXVolumeTexts.Length; i++)
            {
                soundFXVolumeSliders[i].value = soundFXVolume;
                soundFXVolumeTexts[i].text = (soundFXVolume * 10).ToString("F0");
            }

        }
        else
        {
            PlayerPrefs.SetFloat("soundFXVolume", 1);
            SetSoundFXVolume(1);
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("musicVolume");
            SetMusicVolume(musicVolume);
            for (int i = 0; i < musicVolumeTexts.Length; i++)
            {
                musicVolumeSliders[i].value = musicVolume;
                musicVolumeTexts[i].text = (musicVolume * 10).ToString("F0");
            }

        }
        else
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            SetMusicVolume(1);
        }

        // Add listeners to sliders
        //soundFXVolumeSlider.onValueChanged.AddListener(SetSoundFXVolume);
        //musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("masterVolume", level);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("soundFXVolume", level);
        for(int i = 0; i < musicVolumeTexts.Length; i++)
        {
            //soundFXVolumeSliders[i].value = Mathf.Log10(level) * 20f;
            soundFXVolumeTexts[i].text = (level * 10).ToString("F0");
        }
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("musicVolume", level);
        for (int i = 0;i < musicVolumeTexts.Length;i++)
        {
            //musicVolumeSliders[i].value = Mathf.Log10(level) * 20f;
            musicVolumeTexts[i].text = (level * 10).ToString("F0");
        }
        
    }
}
