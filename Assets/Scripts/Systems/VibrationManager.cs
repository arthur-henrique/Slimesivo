using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;
    public bool canVibrate = true;

    private void Awake()
    {
        Vibration.Init();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Vibration"))
        {
            canVibrate = PlayerPrefs.GetInt("Vibration") == 1;
        }
        else
        {
            canVibrate = true;
            PlayerPrefs.SetInt("Vibration", 1);
        }
    }

    public void VibeUI()
    {
        if (canVibrate)
            Vibration.VibrateAndroid(15);
    }

    public void VibeDamage()
    {
        if (canVibrate)
            Vibration.VibrateAndroid(50);
    }

    public void VibeCollectible()
    {
        if (canVibrate)
            Vibration.VibrateAndroid(5);
    }

    public void VibeDeath()
    {
        if (canVibrate)
            Vibration.VibrateAndroid(100);
    }

    public void VibeWin()
    {
        if (canVibrate)
            Vibration.VibrateAndroid(200);
    }

    public void ToggleVibe()
    {
        canVibrate = !canVibrate;
        PlayerPrefs.SetInt("Vibration", canVibrate ? 1 : 0);
        PlayerPrefs.Save();
        print(canVibrate);
    }
}
