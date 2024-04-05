using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;
    private void Awake()
    {
        // Check if instance already exists
        if (Instance == null)
        {
            // If not, set instance to this
            Instance = this;
        }
    }
    void Start()
    {
        Vibration.Init();
    }

    public void CallVibration(int intensity)
    {
        Vibration.VibrateAndroid(intensity);
        Debug.Log("Vibed");
    }

    public void CallPopVibration()
    {
        Vibration.VibratePop();
    }

    public void CallPeekVibration()
    {
        Vibration.VibratePeek();
    }
    
}
