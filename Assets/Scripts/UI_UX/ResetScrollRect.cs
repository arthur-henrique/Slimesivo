using UnityEngine;
using System.Collections;
using UnityEngine.UI;  // Required when Using UI elements.

public class ExampleClass : MonoBehaviour
{
    [SerializeField] ScrollRect myScrollRect;

    void OnEnable()
    {
        //Change the current vertical scroll position --> 0f is the end, 1f is the start
        myScrollRect.verticalNormalizedPosition = 1f;
    }
}