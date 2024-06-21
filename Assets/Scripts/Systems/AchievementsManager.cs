using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowAchievementsUI()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
            Social.ShowAchievementsUI();
    }

    public void UnlockAchievement(string id)
    {
        if(PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Social.ReportProgress(id, 100, success =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked successfully");
                }
                else
                {
                    Debug.Log("Achievement unlock failed");
                }
            });
        }
    }

    public void IncrementAchievement(string id, int stepsToIncrement)
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success =>
            {
                if (success)
                {
                    Debug.Log("Achievement incremented successfully");
                }
                else
                {
                    Debug.Log("Achievement increment failed");
                }
            });
        }
    }
}
