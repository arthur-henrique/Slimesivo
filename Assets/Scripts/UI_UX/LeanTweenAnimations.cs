using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenAnimations : MonoBehaviour
{
    [SerializeField] private bool isThisDetailBackgroundForWinning;
    void Start()
    {
        if (isThisDetailBackgroundForWinning)
        {
            LeanTween.rotateAround(gameObject, Vector3.back, 360, 25f).setLoopClamp();
        }
    }
}
