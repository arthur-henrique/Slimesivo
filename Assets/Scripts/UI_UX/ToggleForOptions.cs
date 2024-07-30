using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleForOptions : MonoBehaviour
{
    [SerializeField] private Toggle toggleCountdown, toggleVibration;
    [SerializeField] private GameObject checkedAssetCountdown, uncheckedAssetCountdown, checkedAssetVibration, uncheckedAssetVibration;
    [SerializeField] private bool isThisCountdown, isThisVibration;

    private void OnEnable()
    {
        if (isThisCountdown)
        {
            // Temporarily disable the listener
            toggleCountdown.onValueChanged.RemoveAllListeners();

            bool countdownPref = Convert.ToBoolean(PlayerPrefs.GetInt("OptionsPreferences"));
            HUDCanvasMenu.playerChoosesCountdown = countdownPref;
            UpdateCountdownUI(countdownPref);
            toggleCountdown.isOn = countdownPref;

            // Register the listener
            toggleCountdown.onValueChanged.AddListener(OnCountdownToggleValueChanged);
        }

        if (isThisVibration)
        {
            // Temporarily disable the listener
            toggleVibration.onValueChanged.RemoveAllListeners();

            bool vibrationPref = PlayerPrefs.GetInt("Vibration", 1) == 1;
            UpdateVibrationUI(vibrationPref);
            toggleVibration.isOn = vibrationPref;

            // Register the listener
            toggleVibration.onValueChanged.AddListener(OnVibrationToggleValueChanged);
        }
    }

    private void OnCountdownToggleValueChanged(bool value)
    {
        HUDCanvasMenu.playerChoosesCountdown = value;
        PlayerPrefs.SetInt("OptionsPreferences", Convert.ToInt32(value));
        PlayerPrefs.Save();
        UpdateCountdownUI(value);
    }

    private void OnVibrationToggleValueChanged(bool value)
    {
        VibrationManager.instance.ToggleVibe(); // Update the VibrationManager's state
        UpdateVibrationUI(value);
    }

    private void UpdateCountdownUI(bool isOn)
    {
        checkedAssetCountdown.SetActive(isOn);
        uncheckedAssetCountdown.SetActive(!isOn);
    }

    private void UpdateVibrationUI(bool isOn)
    {
        checkedAssetVibration.SetActive(isOn);
        uncheckedAssetVibration.SetActive(!isOn);
    }
}
