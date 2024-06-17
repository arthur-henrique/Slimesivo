using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenAnimations : MonoBehaviour
{
    [SerializeField] private bool isThisDetailBackgroundForWinning;
    
    private void OnEnable()
    {
        WinningRotatingRays();   
    }

    private void WinningRotatingRays()
    {
        if (isThisDetailBackgroundForWinning)
        {
            LTDescr tt = LeanTween.rotateAround(gameObject, Vector3.back, 360, 25f).setLoopClamp();
            tt.setIgnoreTimeScale(true);

        }
    }
}
