using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Localization;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;

    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Slider energySlider;
    private int maxEnergy = 10;
    private int currentEnergy;
    private int energyRechargeTime = 4;

    private DateTime nextEnergyTime;
    private DateTime lastEnergyTime;

    private bool isRestoring = false;

    // Localized strings
    public LocalizedString energyFullLocalized;
    public LocalizedString energyReplenishLocalized;

    private string currentLocalizedReplenishString;
    private bool isEnergyFull;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("Subscribing to localized string events.");
        energyFullLocalized.StringChanged += OnEnergyFullStringChanged;
        energyReplenishLocalized.StringChanged += OnEnergyReplenishStringChanged;

        if (!PlayerPrefs.HasKey("currentEnergy"))
        {
            PlayerPrefs.SetInt("currentEnergy", maxEnergy);
        }

        Load();
        UpdateEnergy();
        StartCoroutine(RestoreEnergy());

        if (currentEnergy >= maxEnergy)
        {
            isEnergyFull = true;
            timerText.text = energyFullLocalized.GetLocalizedString();
        }
    }

    private void Update()
    {
        UpdateTimerText();
    }

    private void OnEnergyFullStringChanged(string localizedString)
    {
        if (currentEnergy >= maxEnergy)
        {
            isEnergyFull = true;
            timerText.text = localizedString;
        }
    }

    private void OnEnergyReplenishStringChanged(string localizedString)
    {
        currentLocalizedReplenishString = localizedString;
        if (currentEnergy < maxEnergy)
        {
            isEnergyFull = false;
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        if (isEnergyFull)
        {
            return;
        }

        TimeSpan timeToRecharge = nextEnergyTime - DateTime.Now;
        string timeText = string.Format(currentLocalizedReplenishString, $"{timeToRecharge.Minutes:D2}:{timeToRecharge.Seconds:D2}");
        timerText.text = timeText;
    }

    private void UpdateEnergyTimer()
    {
        if (currentEnergy >= maxEnergy)
        {
            isEnergyFull = true;
            energyFullLocalized.GetLocalizedString();
            return;
        }

        isEnergyFull = false;
        energyReplenishLocalized.GetLocalizedString();
    }

    public bool UseEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            UpdateEnergy();
            if (!isRestoring)
            {
                if (currentEnergy + 1 == maxEnergy)
                {
                    nextEnergyTime = AddDuration(DateTime.Now, energyRechargeTime);
                }
            }
            StartCoroutine(RestoreEnergy());
            return true;
        }
        else
        {
            Debug.Log("Not enough energy");
            // GameManagerMainMenuCanvasScript.Instance.ShowOrHideEnergyPopUpPanel();
            return false;
        }
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;
        UpdateEnergy();
        Save();
    }

    private DateTime AddDuration(DateTime datetime, int duration)
    {
        return datetime.AddMinutes(duration);
    }

    private IEnumerator RestoreEnergy()
    {
        UpdateEnergyTimer();
        isRestoring = true;
        while (currentEnergy < maxEnergy)
        {
            DateTime currentDatetime = DateTime.Now;
            DateTime nextDatetime = nextEnergyTime;
            bool isEnergyAdding = false;

            while (currentDatetime > nextDatetime)
            {
                if (currentEnergy < maxEnergy)
                {
                    isEnergyAdding = true;
                    currentEnergy++;
                    UpdateEnergy();
                    DateTime timeToAdd = lastEnergyTime > nextDatetime ? lastEnergyTime : nextDatetime;
                    nextDatetime = AddDuration(timeToAdd, energyRechargeTime);
                }
                else
                {
                    break;
                }
            }

            if (isEnergyAdding)
            {
                lastEnergyTime = DateTime.Now;
                nextEnergyTime = nextDatetime;
            }

            UpdateEnergyTimer();
            UpdateEnergy();
            Save();
            yield return null;

            isRestoring = false;
        }
        isRestoring = false;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("currentEnergy", currentEnergy);
        PlayerPrefs.SetString("nextEnergyTime", nextEnergyTime.ToString());
        PlayerPrefs.SetString("lastEnergyTime", lastEnergyTime.ToString());
    }

    private void Load()
    {
        currentEnergy = PlayerPrefs.GetInt("currentEnergy");
        nextEnergyTime = StringToDate(PlayerPrefs.GetString("nextEnergyTime"));
        lastEnergyTime = StringToDate(PlayerPrefs.GetString("lastEnergyTime"));
    }

    private DateTime StringToDate(string datetime)
    {
        if (String.IsNullOrEmpty(datetime))
        {
            return DateTime.Now;
        }
        else
        {
            return DateTime.Parse(datetime);
        }
    }

    private void UpdateEnergy()
    {
        energyText.text = currentEnergy + "/" + maxEnergy;
        energySlider.value = currentEnergy;
        energySlider.maxValue = maxEnergy;

        // Check and update the localized string based on the current energy level
        UpdateEnergyTimer();
    }

    private void OnDestroy()
    {
        energyFullLocalized.StringChanged -= OnEnergyFullStringChanged;
        energyReplenishLocalized.StringChanged -= OnEnergyReplenishStringChanged;
    }
}
