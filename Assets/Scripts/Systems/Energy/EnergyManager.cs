using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Slider energySlider;
    private int maxEnergy = 10;
    private int currentEnergy;
    private int energyRechargeTime = 5;

    private DateTime nextEnergyTime;
    private DateTime lastEnergyTime;

    private bool isRestoring = false;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("currentEnergy"))
        {
            PlayerPrefs.SetInt("currentEnergy", maxEnergy);
            Load();
            StartCoroutine(RestoreEnergy());
        }

        else
        {
            Load();
            StartCoroutine(RestoreEnergy());
        }

    }

    #region Internet Start
    //private IEnumerator Start()
    //{
    //    if (!PlayerPrefs.HasKey("currentEnergy"))
    //    {
    //        PlayerPrefs.SetInt("currentEnergy", maxEnergy);
    //        if (Application.internetReachability != NetworkReachability.NotReachable)
    //        {
    //            yield return StartCoroutine(GetInternetTime());
    //        }
    //        else
    //        {
    //            // Fallback to local time
    //            lastEnergyTime = DateTime.Now;
    //            nextEnergyTime = AddDuration(lastEnergyTime, energyRechargeTime);
    //            NotifyPlayerNoInternet();
    //        }
    //        StartCoroutine(RestoreEnergy());
    //    }
    //    else
    //    {
    //        Load();
    //        StartCoroutine(RestoreEnergy());
    //    }
    //}

    #endregion
    public void UseEnergy(int amount)
    {
        if(currentEnergy >= amount)
        {
            currentEnergy -= amount;
            UpdateEnergy();
            if(!isRestoring)
            {
                if(currentEnergy + 1 == maxEnergy)
                {
                    nextEnergyTime = AddDuration(DateTime.Now, energyRechargeTime);
                }
            }
            StartCoroutine(RestoreEnergy());
        }

        else
        {
            Debug.Log("Not enough energy");
        }
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
                if(currentEnergy < maxEnergy)
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

            if(isEnergyAdding)
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

    #region Save&Load

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
        if(String.IsNullOrEmpty(datetime))
        {
            return DateTime.Now;
        }

        else
        {
            return DateTime.Parse(datetime);
        }
    }
    #endregion

    #region UpdateUI and Timer
    private void UpdateEnergyTimer()
    {
        if (currentEnergy >= maxEnergy)
        {
            timerText.text = "Full";
            return;
        }

        TimeSpan timeToRecharge = nextEnergyTime - DateTime.Now;
        string timeText = string.Format("{0:D2}:{1:D2}", timeToRecharge.Minutes, timeToRecharge.Seconds);
        timerText.text = timeText;
    }

    private void UpdateEnergy()
    {
        energyText.text = currentEnergy + "/" + maxEnergy;
        energySlider.value = currentEnergy;
        energySlider.maxValue = maxEnergy;
    }

    #endregion

    #region InternetTimer
    private IEnumerator GetInternetTime()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://worldtimeapi.org/api/timezone/Etc/UTC");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error getting time from internet: " + request.error);
            // Fallback to local time
            lastEnergyTime = DateTime.Now;
            nextEnergyTime = AddDuration(lastEnergyTime, energyRechargeTime);
            NotifyPlayerNoInternet();
        }
        else
        {
            string date = request.GetResponseHeader("date");
            DateTime time = DateTime.Parse(date).ToUniversalTime();
            lastEnergyTime = time;
            nextEnergyTime = AddDuration(lastEnergyTime, energyRechargeTime);
        }
    }

    private void NotifyPlayerNoInternet()
    {
        // Implement a notification system to inform the player
        Debug.Log("No internet connection. Time may not be accurate.");
    }

    #endregion
}