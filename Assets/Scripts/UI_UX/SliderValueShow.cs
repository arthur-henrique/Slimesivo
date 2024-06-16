using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderValueShow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider sliderComponent;
    [SerializeField] private TextMeshProUGUI sliderText;
    void Start()
    {
        sliderComponent.onValueChanged.AddListener((v) =>
        {
            sliderText.text = v.ToString("0");
        });
    }
}
