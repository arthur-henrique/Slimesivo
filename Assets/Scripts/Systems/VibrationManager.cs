using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;
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

    public void VibeUI()
    {
        Vibration.VibrateAndroid(15);
    }
    public void VibeDamage()
    {
        Vibration.VibrateAndroid(50);
    }
    public void VibeCollectible()
    {
        Vibration.VibrateAndroid(5);
    }
    public void VibeDeath()
    {
        Vibration.VibrateAndroid(100);
    }
    public void VibeWin()
    {
        Vibration.VibrateAndroid(200);
    }
}
