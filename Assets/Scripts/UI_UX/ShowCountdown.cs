using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCountdown : MonoBehaviour
{
    public Toggle toggle;          // Reference to the Toggle component
    public GameObject checkedAsset, uncheckedAsset; // Reference to the GameObject to hide/show

    private void Start()
    {
        checkedAsset.SetActive(true);
        uncheckedAsset.SetActive(false);

        toggle.onValueChanged.AddListener((v) =>
        {
            if (toggle.isOn)
            {
                checkedAsset.SetActive(true);
                uncheckedAsset.SetActive(false);
            }
            else
            {
                checkedAsset.SetActive(false);
                uncheckedAsset.SetActive(true);
            }
        });
    }
}
