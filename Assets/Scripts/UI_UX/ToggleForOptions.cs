using System;
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
            if (HUDCanvasMenu.playerChoosesCountdown = Convert.ToBoolean(PlayerPrefs.GetInt("OptionsPreferences")) == true)
            {
                checkedAssetCountdown.SetActive(true);
                uncheckedAssetCountdown.SetActive(false);
            }
            else if (HUDCanvasMenu.playerChoosesCountdown = Convert.ToBoolean(PlayerPrefs.GetInt("OptionsPreferences")) == false)
            {
                checkedAssetCountdown.SetActive(false);
                uncheckedAssetCountdown.SetActive(true);
            }

            toggleCountdown.onValueChanged.AddListener((v) =>
            {
                if (uncheckedAssetCountdown.activeSelf == true)
                {
                    checkedAssetCountdown.SetActive(true);
                    uncheckedAssetCountdown.SetActive(false);
                    HUDCanvasMenu.playerChoosesCountdown = true;
                    PlayerPrefs.SetInt("OptionsPreferences", Convert.ToInt32(HUDCanvasMenu.playerChoosesCountdown));
                    PlayerPrefs.Save();
                }
                else if (checkedAssetCountdown.activeSelf == true)
                {
                    checkedAssetCountdown.SetActive(false);
                    uncheckedAssetCountdown.SetActive(true);
                    HUDCanvasMenu.playerChoosesCountdown = false;

                    PlayerPrefs.SetInt("OptionsPreferences", Convert.ToInt32(HUDCanvasMenu.playerChoosesCountdown));
                    PlayerPrefs.Save();
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