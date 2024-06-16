using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleForOptions : MonoBehaviour
{
    [SerializeField] private Toggle toggleCountdown, toggleVibration;          // Reference to the Toggle component
    [SerializeField] private GameObject checkedAssetCountdown, uncheckedAssetCountdown, checkedAssetVibration, uncheckedAssetVibration; // Reference to the GameObject to hide/show
    [SerializeField] private bool isThisCountdown, isThisVibration;

    private void Start()
    {

        if (isThisCountdown == true)
        {
            checkedAssetCountdown.SetActive(true);
            uncheckedAssetCountdown.SetActive(false);

            toggleCountdown.onValueChanged.AddListener((v) =>
            {
                if (toggleCountdown.isOn)
                {
                    checkedAssetCountdown.SetActive(true);
                    uncheckedAssetCountdown.SetActive(false);
                    HUDCanvasMenu.playerChoosesCountdown = true;
                }
                else
                {
                    checkedAssetCountdown.SetActive(false);
                    uncheckedAssetCountdown.SetActive(true);
                    HUDCanvasMenu.playerChoosesCountdown = false;
                }
            });
        }

        if (isThisVibration == true)
        {
            checkedAssetVibration.SetActive(true);
            uncheckedAssetVibration.SetActive(false);

            toggleVibration.onValueChanged.AddListener((v) =>
            {
                if (toggleVibration.isOn)
                {
                    checkedAssetVibration.SetActive(true);
                    uncheckedAssetVibration.SetActive(false);
                }
                else
                {
                    checkedAssetVibration.SetActive(false);
                    uncheckedAssetVibration.SetActive(true);
                }
            });
        }
    }
}

//playerChoosesCountdown